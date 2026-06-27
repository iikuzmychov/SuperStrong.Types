using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Querying;

public abstract partial class AsPrimitiveTranslationTests(DatabaseHarness database)
    : RelationalTest<AsPrimitiveTranslationTests.Context>(database)
{
    [StrongType<int>] public sealed partial class Rank;
    [StrongType<string>] public sealed partial class Code;
    [StrongType<Guid>] public sealed partial class Tenant;
    [StrongType<decimal>] public sealed partial class Amount;
    [StrongType<DateTime>] public sealed partial class ShippedAt;

    public sealed class Sale
    {
        public int Id { get; set; }
        public Rank Rank { get; set; } = null!;
        public Code Code { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public Amount Amount { get; set; } = null!;
        public ShippedAt? ShippedAt { get; set; }
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Sale> Sales => Set<Sale>();
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task AsPrimitive_translates_in_filter_projection_and_ordering()
    {
        await SeedRanksAsync();

        await using var context = CreateDbContext();

        var results = await context.Sales
            .Where(sale => sale.Rank.AsPrimitive() > 1 && sale.Code.AsPrimitive() != "charlie")
            .OrderByDescending(sale => sale.Amount.AsPrimitive())
            .Select(sale => new { Rank = sale.Rank.AsPrimitive(), Code = sale.Code.AsPrimitive() })
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Collection(
            results,
            result => { Assert.Equal(3, result.Rank); Assert.Equal("bravo", result.Code); },
            result => { Assert.Equal(2, result.Rank); Assert.Equal("alpha", result.Code); });
    }

    [Fact]
    public async Task AsPrimitive_translates_in_aggregates()
    {
        await SeedRanksAsync();

        await using var context = CreateDbContext();
        var cancellationToken = TestContext.Current.CancellationToken;

        Assert.Equal(60m, await context.Sales.SumAsync(sale => sale.Amount.AsPrimitive(), cancellationToken));
        Assert.Equal(30m, await context.Sales.MaxAsync(sale => sale.Amount.AsPrimitive(), cancellationToken));
        Assert.Equal(1, await context.Sales.MinAsync(sale => sale.Rank.AsPrimitive(), cancellationToken));
        Assert.Equal(20m, await context.Sales.AverageAsync(sale => sale.Amount.AsPrimitive(), cancellationToken));
    }

    [Fact]
    public async Task AsPrimitive_and_strong_types_translate_in_group_by()
    {
        var tenantA = Tenant.From(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        var tenantB = Tenant.From(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        await using (var context = CreateDbContext())
        {
            context.Sales.AddRange(
                NewSale(1, "a1", tenantA, 10m),
                NewSale(2, "a2", tenantA, 15m),
                NewSale(3, "b1", tenantB, 20m));
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            var byPrimitive = await context.Sales
                .GroupBy(sale => sale.Tenant.AsPrimitive())
                .Select(group => new { group.Key, Count = group.Count(), Total = group.Sum(sale => sale.Amount.AsPrimitive()) })
                .OrderByDescending(group => group.Count)
                .ToListAsync(cancellationToken);
            Assert.Collection(
                byPrimitive,
                group => { Assert.Equal(tenantA.AsPrimitive(), group.Key); Assert.Equal(2, group.Count); Assert.Equal(25m, group.Total); },
                group => { Assert.Equal(tenantB.AsPrimitive(), group.Key); Assert.Equal(1, group.Count); });

            var byStrongType = await context.Sales
                .GroupBy(sale => sale.Tenant)
                .Select(group => new { group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .ToListAsync(cancellationToken);
            Assert.Equal(tenantA, byStrongType[0].Key);
            Assert.Equal(2, byStrongType[0].Count);
        }
    }

    [Fact]
    public async Task AsPrimitive_translates_in_string_operations()
    {
        await using (var context = CreateDbContext())
        {
            context.Sales.AddRange(
                NewSale(1, "alpha", Tenant.From(Guid.Empty), 1m),
                NewSale(2, "albatross", Tenant.From(Guid.Empty), 1m),
                NewSale(3, "beta", Tenant.From(Guid.Empty), 1m));
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            var startingWithAl = await context.Sales
                .Where(sale => sale.Code.AsPrimitive().StartsWith("al"))
                .Select(sale => sale.Code.AsPrimitive())
                .OrderBy(code => code)
                .ToListAsync(cancellationToken);
            Assert.Equal(["albatross", "alpha"], startingWithAl);

            Assert.Equal(1, await context.Sales.CountAsync(sale => sale.Code.AsPrimitive().Length > 5, cancellationToken));
        }
    }

    [Fact]
    public async Task AsPrimitive_on_a_nullable_column_excludes_null_rows_from_comparisons()
    {
        await using (var context = CreateDbContext())
        {
            context.Sales.AddRange(
                NewSale(1, "old", Tenant.From(Guid.Empty), 1m, new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                NewSale(2, "new", Tenant.From(Guid.Empty), 1m, new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc)),
                NewSale(3, "pending", Tenant.From(Guid.Empty), 1m, shippedAt: null));
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var cancellationToken = TestContext.Current.CancellationToken;
            var threshold = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var recent = await context.Sales
                .Where(sale => sale.ShippedAt!.AsPrimitive() >= threshold)
                .Select(sale => sale.Rank.AsPrimitive())
                .ToListAsync(cancellationToken);
            Assert.Equal([2], recent);
        }
    }

    [Fact]
    public async Task AsPrimitive_translates_in_a_contains_filter()
    {
        await SeedRanksAsync();

        await using var context = CreateDbContext();

        var wanted = new[] { 1, 3 };
        var ranks = await context.Sales
            .Where(sale => wanted.Contains(sale.Rank.AsPrimitive()))
            .Select(sale => sale.Rank.AsPrimitive())
            .OrderBy(rank => rank)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Equal([1, 3], ranks);
    }

    [Fact]
    public async Task AsPrimitive_translates_in_execute_update_with_a_strong_type_value()
    {
        await SeedRanksAsync();

        await using var context = CreateDbContext();
        var cancellationToken = TestContext.Current.CancellationToken;

        var updated = await context.Sales
            .Where(sale => sale.Rank.AsPrimitive() >= 2)
            .ExecuteUpdateAsync(setters => setters.SetProperty(sale => sale.Amount, Amount.From(0m)), cancellationToken);

        Assert.Equal(2, updated);
        Assert.Equal(2, await context.Sales.CountAsync(sale => sale.Amount.AsPrimitive() == 0m, cancellationToken));
    }

    [Fact]
    public async Task AsPrimitive_translates_in_execute_delete()
    {
        await SeedRanksAsync();

        await using var context = CreateDbContext();
        var cancellationToken = TestContext.Current.CancellationToken;

        var deleted = await context.Sales
            .Where(sale => sale.Rank.AsPrimitive() == 3)
            .ExecuteDeleteAsync(cancellationToken);

        Assert.Equal(1, deleted);
        Assert.Equal(2, await context.Sales.CountAsync(cancellationToken));
    }

    private static Sale NewSale(int rank, string code, Tenant tenant, decimal amount, DateTime? shippedAt = null) => new()
    {
        Rank = Rank.From(rank),
        Code = Code.From(code),
        Tenant = tenant,
        Amount = Amount.From(amount),
        ShippedAt = shippedAt is { } value ? ShippedAt.From(value) : null,
    };

    private async Task SeedRanksAsync()
    {
        await using var context = CreateDbContext();

        context.Sales.AddRange(
            NewSale(1, "charlie", Tenant.From(Guid.Empty), 30m),
            NewSale(2, "alpha", Tenant.From(Guid.Empty), 10m),
            NewSale(3, "bravo", Tenant.From(Guid.Empty), 20m));

        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }
}

public sealed class AsPrimitiveTranslationNpgsqlTests(PostgresDatabaseFixture database)
    : AsPrimitiveTranslationTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class AsPrimitiveTranslationSqlServerTests(SqlServerDatabaseFixture database)
    : AsPrimitiveTranslationTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
