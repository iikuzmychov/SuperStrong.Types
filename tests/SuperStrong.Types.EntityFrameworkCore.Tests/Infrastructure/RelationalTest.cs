using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

// Provider-agnostic base for a single focused scenario: it owns one small DbContext and runs on
// every provider through a DatabaseHarness supplied by a thin per-provider subclass.
public abstract class RelationalTest<TContext>(DatabaseHarness database) : IAsyncLifetime
    where TContext : DbContext
{
    protected TContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>()
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));

        database.Configure(optionsBuilder);
        optionsBuilder.UseStrongTypes();

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
