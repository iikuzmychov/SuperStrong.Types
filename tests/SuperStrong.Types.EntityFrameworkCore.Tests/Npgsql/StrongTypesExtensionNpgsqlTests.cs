using Microsoft.EntityFrameworkCore;
using SuperStrong.Tests.ExampleTypes;
using SuperStrong.Types.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public sealed class StrongTypesExtensionNpgsqlTests(ITestOutputHelper testOutputHelper)
    : PostgresContainerTest(testOutputHelper)
{
    [Fact]
    public async Task Should_perform_query_with_strong_type_conversion()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(Container.GetConnectionString())
            .UseStrongTypes()
            .Options;

        using (var context = new TestDbContext(options))
        {
            await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

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

        using (var context = new TestDbContext(options))
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

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
    }

    private sealed class User(UserId id)
    {
        public UserId Id { get; private set; } = id ?? throw new ArgumentNullException(nameof(id));
    }
}
