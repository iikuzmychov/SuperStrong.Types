using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract partial class OwnedTypeTests(DatabaseHarness database)
    : RelationalTest<OwnedTypeTests.Context>(database)
{
    [StrongType<string>] public sealed partial class City;
    [StrongType<string>] public sealed partial class Zip;

    public sealed class Address
    {
        public City City { get; set; } = null!;
        public Zip Zip { get; set; } = null!;
    }

    public sealed class Stop
    {
        public City City { get; set; } = null!;
    }

    public sealed class Shipment
    {
        public int Id { get; set; }
        public Address Origin { get; set; } = null!;
        public IList<Stop> Stops { get; set; } = [];
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Shipment> Shipments => Set<Shipment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Shipment>(shipment =>
            {
                shipment.OwnsOne(entity => entity.Origin);
                shipment.OwnsMany(entity => entity.Stops);
            });
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task Strong_types_inside_an_owned_reference_persist_and_read_back()
    {
        await using (var context = CreateDbContext())
        {
            context.Shipments.Add(new Shipment { Origin = new Address { City = City.From("a"), Zip = Zip.From("b") } });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var shipment = await context.Shipments.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(City.From("a"), shipment.Origin.City);
            Assert.Equal(Zip.From("b"), shipment.Origin.Zip);
        }
    }

    [Fact]
    public async Task Strong_types_inside_an_owned_collection_persist_and_read_back()
    {
        await using (var context = CreateDbContext())
        {
            context.Shipments.Add(new Shipment
            {
                Origin = new Address { City = City.From("a"), Zip = Zip.From("b") },
                Stops = [new Stop { City = City.From("c") }, new Stop { City = City.From("d") }],
            });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var shipment = await context.Shipments.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal([City.From("c"), City.From("d")], shipment.Stops.Select(stop => stop.City));
        }
    }
}

public sealed class OwnedTypeNpgsqlTests(PostgresDatabaseFixture database)
    : OwnedTypeTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class OwnedTypeSqlServerTests(SqlServerDatabaseFixture database)
    : OwnedTypeTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
