using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;
using System.Globalization;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MaxValueValidatorAdapter;

public sealed class MaxValueValidatorAdapterIncompatibleConversionNpgsqlTests(ITestOutputHelper testOutputHelper)
    : NpgsqlValidationAdapterTest<MaxValueValidatorAdapterIncompatibleConversionNpgsqlTests.TestDbContext>(testOutputHelper)
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
            modelBuilder.Entity<Player>()
                .Property(player => player.Score)
                .HasConversion(
                    score => score.AsPrimitive().ToString(CultureInfo.InvariantCulture),
                    stored => Score.Create(int.Parse(stored, CultureInfo.InvariantCulture)));
        }
    }

    protected override void ConfigureOptions(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(typeof(MaxValueValidatorAdapterFactory));
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_provider_type_differs_from_validator_primitive()
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

    // todo: delete manual implementation once source generators is implemented
    public sealed partial class Score : IStrongType<Score, int>
    {
        private readonly int _value;

        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();

        private Score(int value)
        {
            _value = value;
        }

        public static Score Create(int value)
        {
            StrongType.EnsureValid(value, Definition);

            return new Score(value);
        }

        public int AsPrimitive() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object? obj) => obj is Score other && _value == other._value;
    }
}
