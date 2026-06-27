using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Querying;

public abstract partial class FilterTests(DatabaseHarness database)
    : RelationalTest<FilterTests.Context>(database)
{
    [StrongType<string>] public sealed partial class Code;
    [StrongType<decimal>] public sealed partial class Price;

    public sealed class Product
    {
        public int Id { get; set; }
        public Code Code { get; set; } = null!;
        public Price Price { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task Filters_by_strong_type_equality()
    {
        await SeedAsync();

        await using var context = CreateDbContext();

        var product = await context.Products
            .SingleAsync(entity => entity.Code == Code.From("b"), TestContext.Current.CancellationToken);

        Assert.Equal(Price.From(20m), product.Price);
    }

    [Fact]
    public async Task Filters_by_strong_type_relational_operators()
    {
        await SeedAsync();

        await using var context = CreateDbContext();
        var cancellationToken = TestContext.Current.CancellationToken;

        Assert.Equal(2, await context.Products.CountAsync(entity => entity.Price > Price.From(10m), cancellationToken));
        Assert.Equal(1, await context.Products.CountAsync(entity => entity.Price <= Price.From(10m), cancellationToken));
    }

    [Fact]
    public async Task Filters_by_a_strong_type_value_list()
    {
        await SeedAsync();

        await using var context = CreateDbContext();

        var wanted = new[] { Code.From("a"), Code.From("c") };
        var count = await context.Products
            .Where(entity => wanted.Contains(entity.Code))
            .CountAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, count);
    }

    private async Task SeedAsync()
    {
        await using var context = CreateDbContext();

        context.Products.AddRange(
            new Product { Code = Code.From("a"), Price = Price.From(10m) },
            new Product { Code = Code.From("b"), Price = Price.From(20m) },
            new Product { Code = Code.From("c"), Price = Price.From(30m) });

        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }
}

public sealed class FilterNpgsqlTests(PostgresDatabaseFixture database)
    : FilterTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class FilterSqlServerTests(SqlServerDatabaseFixture database)
    : FilterTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
