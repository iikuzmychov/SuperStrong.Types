using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Relationships;

public abstract partial class ForeignKeyTests(DatabaseHarness database)
    : RelationalTest<ForeignKeyTests.Context>(database)
{
    [StrongType<int>] public sealed partial class OrderId;
    [StrongType<string>] public sealed partial class CustomerCode;
    [StrongType<int>] public sealed partial class Quantity;

    public sealed class Order
    {
        public OrderId Id { get; set; } = null!;
        public CustomerCode Customer { get; set; } = null!;
        public ICollection<Line> Lines { get; set; } = [];
    }

    public sealed class Line
    {
        public int Id { get; set; }
        public OrderId OrderId { get; set; } = null!;
        public Quantity Quantity { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Line> Lines => Set<Line>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Order>()
                .HasMany(order => order.Lines)
                .WithOne(line => line.Order)
                .HasForeignKey(line => line.OrderId);
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task Related_entities_are_linked_through_a_strong_type_foreign_key()
    {
        var orderId = OrderId.From(100);

        await using (var context = CreateDbContext())
        {
            context.Orders.Add(new Order { Id = orderId, Customer = CustomerCode.From("a") });
            context.Lines.AddRange(
                new Line { OrderId = orderId, Quantity = Quantity.From(3) },
                new Line { OrderId = orderId, Quantity = Quantity.From(5) });

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
            Assert.Equal(CustomerCode.From("a"), line.Order.Customer);
        }
    }
}

public sealed class ForeignKeyNpgsqlTests(PostgresDatabaseFixture database)
    : ForeignKeyTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ForeignKeySqlServerTests(SqlServerDatabaseFixture database)
    : ForeignKeyTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
