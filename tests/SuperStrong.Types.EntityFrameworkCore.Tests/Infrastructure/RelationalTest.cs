using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

public abstract class RelationalTest<TContext>(DatabaseHarness database) : IAsyncLifetime
    where TContext : DbContext
{
    protected TContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseStrongTypes();

        database.Configure(optionsBuilder);

        return CreateDbContext(optionsBuilder.Options);
    }

    protected abstract TContext CreateDbContext(DbContextOptions<TContext> options);

    public async ValueTask InitializeAsync()
    {
        await using var context = CreateDbContext();
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

        await database.ResetAsync();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }
}
