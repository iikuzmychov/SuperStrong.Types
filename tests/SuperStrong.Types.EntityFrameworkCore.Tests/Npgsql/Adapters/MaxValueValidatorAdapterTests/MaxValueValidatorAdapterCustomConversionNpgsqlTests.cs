using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MaxValueValidatorAdapterTests;

public sealed partial class MaxValueValidatorAdapterCustomConversionNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MaxValueValidatorAdapterCustomConversionNpgsqlTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Score : IHasStrongTypeDefinition<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMaxValue(100);
    }

    public sealed class Player
    {
        public required int Id { get; init; }
        public required Score Score { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Player>()
                .Property(player => player.Score)
                .HasConversion(
                    score => score.AsPrimitive() * 10,
                    stored => Score.Create(stored / 10));
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MaxValueValidatorAdapterFactory());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_has_user_supplied_value_converter()
    {
        await using var connection = Context.Database.GetDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = """
            select count(*)
            from pg_constraint constraint_
            join pg_class table_ on constraint_.conrelid = table_.oid
            where table_.relname = 'Player' and constraint_.contype = 'c'
            """;

        var count = (long)(await command.ExecuteScalarAsync(TestContext.Current.CancellationToken))!;

        Assert.Equal(0, count);
    }

}
