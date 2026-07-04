using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract class RoundTripTests<TStrongType, TPrimitive, TSamples>(DatabaseHarness database)
    : RelationalTest<RoundTripTests<TStrongType, TPrimitive, TSamples>.Context>(database)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TSamples : TheoryData<TPrimitive>, new()
{
    public sealed class Entity
    {
        public int Id { get; set; }
        public TStrongType Value { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Entity> Entities => Set<Entity>();
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    public static TheoryData<TPrimitive> Primitives { get; } = new TSamples();

    [Theory]
    [MemberData(nameof(Primitives))]
    public async Task A_strong_type_value_persists_and_reads_back_unchanged(TPrimitive primitive)
    {
        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new Entity { Value = TStrongType.From(primitive) });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(primitive, entity.Value.AsPrimitive());
        }
    }
}
