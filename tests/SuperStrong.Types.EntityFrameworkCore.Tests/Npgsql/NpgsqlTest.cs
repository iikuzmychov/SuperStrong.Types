using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public abstract class NpgsqlTest<TDbContext>(PostgresDatabaseFixture database)
    : IAsyncLifetime, IClassFixture<PostgresDatabaseFixture>
    where TDbContext : DbContext
{
    public async ValueTask InitializeAsync()
    {
        await using var bootstrapContext = CreateDbContext();
        await bootstrapContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

        await database.ResetAsync();

        await InitializeCoreAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCoreAsync();

        GC.SuppressFinalize(this);
    }

    protected TDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql($"{database.ConnectionString};Include Error Detail=true")
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));

        ConfigureDbContext(optionsBuilder);

        return CreateDbContext(optionsBuilder.Options);
    }

    protected virtual ValueTask InitializeCoreAsync() => ValueTask.CompletedTask;

    protected virtual ValueTask DisposeCoreAsync() => ValueTask.CompletedTask;

    protected virtual void ConfigureDbContext(DbContextOptionsBuilder<TDbContext> options)
    {
    }

    protected abstract TDbContext CreateDbContext(DbContextOptions<TDbContext> options);
}
