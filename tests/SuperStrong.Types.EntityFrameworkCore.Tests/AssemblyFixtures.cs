using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

[assembly: AssemblyFixture(typeof(PostgresContainerFixture))]
[assembly: AssemblyFixture(typeof(SqlServerContainerFixture))]
