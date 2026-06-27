using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Keys;

public abstract partial class CompositeKeyTests(DatabaseHarness database)
    : RelationalTest<CompositeKeyTests.Context>(database)
{
    [StrongType<int>] public sealed partial class WarehouseId;
    [StrongType<int>] public sealed partial class Bin;
    [StrongType<int>] public sealed partial class Capacity;

    public sealed class StockItem
    {
        public WarehouseId WarehouseId { get; set; } = null!;
        public Bin Bin { get; set; } = null!;
        public Capacity Capacity { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<StockItem> StockItems => Set<StockItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<StockItem>().HasKey(item => new { item.WarehouseId, item.Bin });
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task An_entity_with_a_composite_strong_type_key_can_be_found_by_that_key()
    {
        await using (var context = CreateDbContext())
        {
            context.StockItems.AddRange(
                new StockItem { WarehouseId = WarehouseId.From(1), Bin = Bin.From(10), Capacity = Capacity.From(100) },
                new StockItem { WarehouseId = WarehouseId.From(1), Bin = Bin.From(20), Capacity = Capacity.From(200) },
                new StockItem { WarehouseId = WarehouseId.From(2), Bin = Bin.From(10), Capacity = Capacity.From(300) });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var item = await context.StockItems.FindAsync(
                [WarehouseId.From(1), Bin.From(20)],
                TestContext.Current.CancellationToken);

            Assert.NotNull(item);
            Assert.Equal(Capacity.From(200), item.Capacity);
        }
    }
}

public sealed class CompositeKeyNpgsqlTests(PostgresDatabaseFixture database)
    : CompositeKeyTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class CompositeKeySqlServerTests(SqlServerDatabaseFixture database)
    : CompositeKeyTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
