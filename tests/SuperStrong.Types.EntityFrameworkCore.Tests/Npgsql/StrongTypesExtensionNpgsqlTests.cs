using Microsoft.EntityFrameworkCore;
using SuperStrong.Tests.ExampleTypes;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public sealed class StrongTypesExtensionNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlTest<StrongTypesExtensionNpgsqlTests.TestDbContext>(database)
{
    protected override void ConfigureDbContext(DbContextOptionsBuilder<TestDbContext> options)
    {
        options.UseStrongTypes();
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Should_perform_query_with_strong_type_conversion()
    {
        await using (var context = CreateDbContext())
        {
            context.Set<User>().AddRange(
            [
                new(UserId.Create(1)),
                new(UserId.Create(2)),
                new(UserId.Create(3)),
                new(UserId.Create(4)),
                new(UserId.Create(5)),
            ]);

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var users = await context
                .Set<User>()
                .Where(user => user.Id.AsPrimitive() > 3)
                .ToListAsync(TestContext.Current.CancellationToken);

            Assert.Collection(
                users,
                user => Assert.Equal(UserId.Create(4), user.Id),
                user => Assert.Equal(UserId.Create(5), user.Id));
        }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
    }

    public sealed class User(UserId id)
    {
        public UserId Id { get; private set; } = id ?? throw new ArgumentNullException(nameof(id));
    }
}
