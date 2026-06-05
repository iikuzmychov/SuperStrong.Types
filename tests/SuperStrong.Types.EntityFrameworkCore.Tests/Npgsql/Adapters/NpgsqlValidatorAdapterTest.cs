using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters;

public abstract class NpgsqlValidatorAdapterTest<TDbContext>(PostgresDatabaseFixture database)
    : NpgsqlTest<TDbContext>(database)
    where TDbContext : DbContext
{
    protected TDbContext Context { get; private set; } = null!;

    protected sealed override ValueTask InitializeCoreAsync()
    {
        Context = CreateDbContext();

        return ValueTask.CompletedTask;
    }

    protected sealed override async ValueTask DisposeCoreAsync()
    {
        if (Context is not null)
        {
            await Context.DisposeAsync();
        }
    }

    protected sealed override void ConfigureDbContext(DbContextOptionsBuilder<TDbContext> options)
    {
        options.UseStrongTypes(ConfigureStrongTypes);
    }

    protected abstract void ConfigureStrongTypes(StrongTypeOptionsBuilder options);
}
