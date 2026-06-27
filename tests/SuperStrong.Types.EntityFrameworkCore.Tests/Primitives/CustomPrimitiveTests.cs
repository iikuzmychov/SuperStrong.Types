using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Primitives;

public abstract partial class CustomPrimitiveTests(DatabaseHarness database)
    : RelationalTest<CustomPrimitiveTests.Context>(database)
{
    public readonly record struct Rgb(byte R, byte G, byte B)
    {
        public override string ToString() => $"{R:X2}{G:X2}{B:X2}";

        public static Rgb Parse(string value) => new(
            Convert.ToByte(value.Substring(0, 2), 16),
            Convert.ToByte(value.Substring(2, 2), 16),
            Convert.ToByte(value.Substring(4, 2), 16));
    }

    [StrongType<Rgb>] public sealed partial class Swatch;

    public sealed class Palette
    {
        public int Id { get; set; }
        public Swatch Primary { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Palette> Palettes => Set<Palette>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Palette>()
                .Property(palette => palette.Primary)
                .HasConversion(
                    swatch => swatch.AsPrimitive().ToString(),
                    value => Swatch.From(Rgb.Parse(value)));
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task A_strong_type_over_an_unmappable_custom_primitive_round_trips_via_a_property_converter()
    {
        var swatch = Swatch.From(new Rgb(0x12, 0x34, 0x56));

        await using (var context = CreateDbContext())
        {
            context.Palettes.Add(new Palette { Primary = swatch });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var palette = await context.Palettes.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(swatch, palette.Primary);
        }
    }
}

public sealed class CustomPrimitiveNpgsqlTests(PostgresDatabaseFixture database)
    : CustomPrimitiveTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class CustomPrimitiveSqlServerTests(SqlServerDatabaseFixture database)
    : CustomPrimitiveTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
