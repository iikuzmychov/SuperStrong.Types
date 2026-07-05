using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Data.Common;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

public abstract class DatabaseFixture : IAsyncLifetime
{
    private readonly string _databaseName = $"db_{Guid.NewGuid():N}";
    private Respawner? _respawner;

    public string ConnectionString { get; private set; } = null!;

    public abstract void Configure(DbContextOptionsBuilder builder);

    protected abstract string AdminConnectionString { get; }
    protected abstract IDbAdapter DbAdapter { get; }

    protected abstract DbConnection CreateConnection(string connectionString);
    protected abstract string GetDatabaseConnectionString(string adminConnectionString, string databaseName);
    protected abstract string GetCreateDatabaseCommandText(string databaseName);
    protected abstract string GetDropDatabaseCommandText(string databaseName);

    public async ValueTask InitializeAsync()
    {
        await ExecuteAdminCommandAsync(GetCreateDatabaseCommandText(_databaseName));

        ConnectionString = GetDatabaseConnectionString(AdminConnectionString, _databaseName);
    }

    public async ValueTask ResetAsync()
    {
        await using var connection = CreateConnection(ConnectionString);
        await connection.OpenAsync();

        if (_respawner is null)
        {
            try
            {
                _respawner = await Respawner.CreateAsync(
                    connection,
                    new RespawnerOptions
                    {
                        DbAdapter = DbAdapter,
                        WithReseed = true,
                    });
            }
            catch (InvalidOperationException)
            {
                // nothing to reset
            }

            return;
        }

        await _respawner.ResetAsync(connection);
    }

    public async ValueTask DisposeAsync()
    {
        await ExecuteAdminCommandAsync(GetDropDatabaseCommandText(_databaseName));
    }

    private async Task ExecuteAdminCommandAsync(string commandText)
    {
        await using var connection = CreateConnection(AdminConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = commandText;

        await command.ExecuteNonQueryAsync();
    }
}
