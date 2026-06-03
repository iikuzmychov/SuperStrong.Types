using Testcontainers.PostgreSql;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public sealed class PostgresContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("postgres:18").Build();

    public async ValueTask InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
