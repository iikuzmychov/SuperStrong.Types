using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

public abstract class DatabaseHarness
{
    public abstract void Configure(DbContextOptionsBuilder builder);

    public abstract ValueTask ResetAsync();
}

public sealed class NpgsqlHarness(PostgresDatabaseFixture database) : DatabaseHarness
{
    public override void Configure(DbContextOptionsBuilder builder)
        => builder.UseNpgsql($"{database.ConnectionString};Include Error Detail=true");

    public override ValueTask ResetAsync() => database.ResetAsync();
}

public sealed class SqlServerHarness(SqlServerDatabaseFixture database) : DatabaseHarness
{
    public override void Configure(DbContextOptionsBuilder builder)
        => builder.UseSqlServer(database.ConnectionString);

    public override ValueTask ResetAsync() => database.ResetAsync();
}
