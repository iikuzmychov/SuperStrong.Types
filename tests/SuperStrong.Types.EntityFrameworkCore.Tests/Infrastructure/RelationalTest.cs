using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

public abstract class RelationalTest<TContext>(DatabaseFixture database) : IAsyncLifetime
    where TContext : DbContext
{
    protected TContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseStrongTypes();

        database.Configure(optionsBuilder);

        return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
    }

    public async ValueTask InitializeAsync()
    {
        await using var context = CreateDbContext();
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

        await database.ResetAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
