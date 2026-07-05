using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract partial class CollectionTests(DatabaseFixture database)
    : RelationalTest<CollectionTests.Context>(database)
{
    [StrongType<string>] public sealed partial class Tag;

    public sealed class Article
    {
        public int Id { get; set; }
        public IList<Tag> Tags { get; set; } = [];
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Article> Articles => Set<Article>();
    }

    [Fact]
    public async Task A_collection_of_strong_types_persists_and_reads_back_in_order()
    {
        await using (var context = CreateDbContext())
        {
            context.Articles.Add(new() { Tags = [Tag.From("a"), Tag.From("b"), Tag.From("c")] });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var article = await context.Articles.SingleAsync(TestContext.Current.CancellationToken);

            Assert.Equal([Tag.From("a"), Tag.From("b"), Tag.From("c")], article.Tags);
        }
    }

    [Fact]
    public async Task Rows_can_be_filtered_by_strong_type_collection_membership()
    {
        await using (var context = CreateDbContext())
        {
            context.Articles.AddRange(
            [
                new() { Tags = [Tag.From("a"), Tag.From("b")] },
                new() { Tags = [Tag.From("c")] },
            ]);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var matches = await context.Articles
                .CountAsync(
                    article => article.Tags.Contains(Tag.From("b")),
                    TestContext.Current.CancellationToken);

            Assert.Equal(1, matches);
        }
    }
}

public sealed class CollectionNpgsqlTests(NpgsqlDatabaseFixture database)
    : CollectionTests(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class CollectionSqlServerTests(SqlServerDatabaseFixture database)
    : CollectionTests(database), IClassFixture<SqlServerDatabaseFixture>;
