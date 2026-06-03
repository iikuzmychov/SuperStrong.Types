using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public abstract class NpgsqlValidationAdapterTest<TDbContext>(ITestOutputHelper testOutputHelper)
    : PostgresContainerTest(testOutputHelper)
    where TDbContext : DbContext
{
    protected TDbContext Context { get; private set; } = null!;

    protected override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql($"{Container.GetConnectionString()};Include Error Detail=true")
            .UseStrongTypes(ConfigureOptions)
            .Options;

        Context = CreateDbContext(options);
        await Context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (Context is not null)
        {
            await Context.DisposeAsync();
        }

        await base.DisposeAsyncCore();
    }

    protected abstract void ConfigureOptions(StrongTypeOptionsBuilder options);

    protected abstract TDbContext CreateDbContext(DbContextOptions<TDbContext> options);
}
