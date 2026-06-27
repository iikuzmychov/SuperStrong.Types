using Testcontainers.MsSql;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; } =
        new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04").Build();

    public async ValueTask InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}
