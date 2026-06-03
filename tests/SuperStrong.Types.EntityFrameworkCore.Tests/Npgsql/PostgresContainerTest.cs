using Testcontainers.PostgreSql;
using Testcontainers.Xunit;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;

public abstract class PostgresContainerTest(ITestOutputHelper testOutputHelper)
    : ContainerTest<PostgreSqlBuilder, PostgreSqlContainer>(testOutputHelper)
{
    protected sealed override PostgreSqlBuilder Configure()
    {
        return new PostgreSqlBuilder("postgres:18");
    }
}
