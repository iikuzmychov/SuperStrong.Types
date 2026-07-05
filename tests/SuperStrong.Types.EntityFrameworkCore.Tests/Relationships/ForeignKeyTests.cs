using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Relationships;

public abstract class ForeignKeyTests<TStrongType, TPrimitive, TSamples>(DatabaseFixture database)
    : RelationalTest<ForeignKeyTests<TStrongType, TPrimitive, TSamples>.Context>(database)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TSamples : TheoryData<TPrimitive>, new()
{
    public sealed class Order
    {
        public TStrongType Id { get; set; } = null!;
        public ICollection<Line> Lines { get; set; } = [];
    }

    public sealed class Line
    {
        public int Id { get; set; }
        public TStrongType OrderId { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Line> Lines => Set<Line>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .Property(order => order.Id)
                .ValueGeneratedNever();

            modelBuilder
                .Entity<Order>()
                .HasMany(order => order.Lines)
                .WithOne(line => line.Order)
                .HasForeignKey(line => line.OrderId);
        }
    }

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TSamples();

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task Related_entities_are_linked_through_a_strong_type_foreign_key(TPrimitive primitive)
    {
        var orderId = TStrongType.From(primitive);

        await using (var context = CreateDbContext())
        {
            context.Orders.Add(new() { Id = orderId });

            context.Lines.AddRange(
            [
                new() { OrderId = orderId },
                new() { OrderId = orderId },
            ]);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var order = await context.Orders
                .Include(entity => entity.Lines)
                .SingleAsync(TestContext.Current.CancellationToken);

            Assert.Equal(orderId, order.Id);
            Assert.Equal(2, order.Lines.Count);
            Assert.All(order.Lines, line => Assert.Equal(orderId, line.OrderId));

            var line = await context.Lines
                .Include(entity => entity.Order)
                .FirstAsync(TestContext.Current.CancellationToken);

            Assert.Equal(orderId, line.Order.Id);
        }
    }
}

public sealed class IntForeignKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : ForeignKeyTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class IntForeignKeySqlServerTests(SqlServerDatabaseFixture database)
    : ForeignKeyTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongForeignKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : ForeignKeyTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class LongForeignKeySqlServerTests(SqlServerDatabaseFixture database)
    : ForeignKeyTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidForeignKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : ForeignKeyTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class GuidForeignKeySqlServerTests(SqlServerDatabaseFixture database)
    : ForeignKeyTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class StringForeignKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : ForeignKeyTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class StringForeignKeySqlServerTests(SqlServerDatabaseFixture database)
    : ForeignKeyTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;
