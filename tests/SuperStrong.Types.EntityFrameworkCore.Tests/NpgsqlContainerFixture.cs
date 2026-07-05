using Testcontainers.PostgreSql;

namespace SuperStrong.Types.EntityFrameworkCore.Tests;

public sealed class NpgsqlContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("postgres:18").Build();

    public async ValueTask InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
