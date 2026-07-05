using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using System.Data.Common;

namespace SuperStrong.Types.EntityFrameworkCore.Tests;

public sealed class NpgsqlDatabaseFixture(NpgsqlContainerFixture container) : DatabaseFixture
{
    public override void Configure(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql($"{ConnectionString};Include Error Detail=true");
    }

    protected override string AdminConnectionString => container.Container.GetConnectionString();
    protected override IDbAdapter DbAdapter => Respawn.DbAdapter.Postgres;

    protected override DbConnection CreateConnection(string connectionString) => new NpgsqlConnection(connectionString);

    protected override string GetDatabaseConnectionString(string adminConnectionString, string databaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(adminConnectionString)
        {
            Database = databaseName,
        };

        return builder.ToString();
    }

    protected override string GetCreateDatabaseCommandText(string databaseName)
        => @$"create database ""{databaseName}""";

    protected override string GetDropDatabaseCommandText(string databaseName)
        => @$"drop database ""{databaseName}"" with (force)";
}
