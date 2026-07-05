using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Keys;

public abstract class KeyTests<TStrongType, TPrimitive, TSamples>(DatabaseFixture database)
    : RelationalTest<KeyTests<TStrongType, TPrimitive, TSamples>.Context>(database)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TSamples : TheoryData<TPrimitive>, new()
{
    public sealed class Entity
    {
        public TStrongType Id { get; set; } = null!;
        public string Name { get; set; } = "";
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Entity> Entities => Set<Entity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Entity>().Property(entity => entity.Id).ValueGeneratedNever();
    }

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TSamples();

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task An_entity_can_be_found_by_its_strong_type_key(TPrimitive primitive)
    {
        var id = TStrongType.From(primitive);

        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new Entity { Id = id, Name = "a" });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.FindAsync([id], TestContext.Current.CancellationToken);

            Assert.NotNull(entity);
            Assert.Equal("a", entity.Name);
        }
    }

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task An_entity_can_be_removed_by_its_strong_type_key(TPrimitive primitive)
    {
        var id = TStrongType.From(primitive);

        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new Entity { Id = id, Name = "a" });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.FindAsync([id], TestContext.Current.CancellationToken);
            context.Entities.Remove(entity!);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            Assert.Null(await context.Entities.FindAsync([id], TestContext.Current.CancellationToken));
        }
    }
}

public sealed class IntKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class IntKeySqlServerTests(SqlServerDatabaseFixture database)
    : KeyTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class LongKeySqlServerTests(SqlServerDatabaseFixture database)
    : KeyTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class GuidKeySqlServerTests(SqlServerDatabaseFixture database)
    : KeyTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class StringKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class StringKeySqlServerTests(SqlServerDatabaseFixture database)
    : KeyTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;
