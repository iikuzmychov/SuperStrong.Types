using Microsoft.Data.SqlClient;
using Respawn;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

public sealed class SqlServerDatabaseFixture(SqlServerContainerFixture container) : IAsyncLifetime
{
    private readonly string _databaseName = $"db_{Guid.NewGuid():N}";
    private Respawner? _respawner;

    public string ConnectionString { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await using var adminConnection = new SqlConnection(container.Container.GetConnectionString());
        await adminConnection.OpenAsync();

        await using var createCommand = adminConnection.CreateCommand();
        createCommand.CommandText = $"create database [{_databaseName}]";

        await createCommand.ExecuteNonQueryAsync();

        var builder = new SqlConnectionStringBuilder(container.Container.GetConnectionString())
        {
            InitialCatalog = _databaseName,
        };

        ConnectionString = builder.ToString();
    }

    public async ValueTask ResetAsync()
    {
        if (_respawner is null)
        {
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            try
            {
                _respawner = await Respawner.CreateAsync(
                    connection,
                    new RespawnerOptions
                    {
                        DbAdapter = DbAdapter.SqlServer,
                        WithReseed = true,
                    });
            }
            catch (InvalidOperationException)
            {
                // nothing to reset
            }

            return;
        }

        await using var resetConnection = new SqlConnection(ConnectionString);
        await resetConnection.OpenAsync();
        await _respawner.ResetAsync(resetConnection);
    }

    public async ValueTask DisposeAsync()
    {
        await using var adminConnection = new SqlConnection(container.Container.GetConnectionString());
        await adminConnection.OpenAsync();

        await using var dropCommand = adminConnection.CreateCommand();
        dropCommand.CommandText = $"""
            alter database [{_databaseName}] set single_user with rollback immediate;
            drop database [{_databaseName}];
            """;

        await dropCommand.ExecuteNonQueryAsync();
    }
}
