using Npgsql;
using Respawn;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public sealed class PostgresDatabaseFixture(PostgresContainerFixture container) : IAsyncLifetime
{
    private readonly string _databaseName = $"db_{Guid.NewGuid():N}";
    private Respawner? _respawner;

    public string ConnectionString { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await using var adminConnection = new NpgsqlConnection(container.Container.GetConnectionString());
        await adminConnection.OpenAsync();

        await using var createCommand = adminConnection.CreateCommand();
        createCommand.CommandText = $"""create database "{_databaseName}" """;

        await createCommand.ExecuteNonQueryAsync();

        var builder = new NpgsqlConnectionStringBuilder(container.Container.GetConnectionString())
        {
            Database = _databaseName,
        };

        ConnectionString = builder.ToString();
    }

    public async ValueTask ResetAsync()
    {
        if (_respawner is null)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            try
            {
                _respawner = await Respawner.CreateAsync(
                    connection,
                    new RespawnerOptions
                    {
                        DbAdapter = DbAdapter.Postgres,
                        WithReseed = true,
                    });
            }
            catch (InvalidOperationException)
            {
                // nothing to reset
            }

            return;
        }

        await using var resetConnection = new NpgsqlConnection(ConnectionString);
        await resetConnection.OpenAsync();
        await _respawner.ResetAsync(resetConnection);
    }

    public async ValueTask DisposeAsync()
    {
        await using var adminConnection = new NpgsqlConnection(container.Container.GetConnectionString());
        await adminConnection.OpenAsync();

        await using var dropCommand = adminConnection.CreateCommand();
        dropCommand.CommandText = $"""drop database "{_databaseName}" with (force)""";

        await dropCommand.ExecuteNonQueryAsync();
    }
}
