using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Data.Common;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

public sealed class SqlServerDatabaseFixture(SqlServerContainerFixture container) : DatabaseFixture
{
    public override void Configure(DbContextOptionsBuilder builder)
        => builder.UseSqlServer(ConnectionString);

    protected override string AdminConnectionString => container.Container.GetConnectionString();
    protected override IDbAdapter DbAdapter => Respawn.DbAdapter.SqlServer;

    protected override DbConnection CreateConnection(string connectionString) => new SqlConnection(connectionString);

    protected override string GetDatabaseConnectionString(string adminConnectionString, string databaseName)
    {
        var builder = new SqlConnectionStringBuilder(adminConnectionString)
        {
            InitialCatalog = databaseName,
        };

        return builder.ToString();
    }

    protected override string GetCreateDatabaseCommandText(string databaseName)
        => $"create database [{databaseName}]";

    protected override string GetDropDatabaseCommandText(string databaseName)
    {
        return $"""
            alter database [{databaseName}] set single_user with rollback immediate;
            drop database [{databaseName}];
            """;
    }
}
