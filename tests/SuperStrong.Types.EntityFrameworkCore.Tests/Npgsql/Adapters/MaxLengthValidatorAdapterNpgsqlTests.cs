using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters;

public sealed class MaxLengthValidatorAdapterNpgsqlTests(ITestOutputHelper testOutputHelper)
    : NpgsqlValidationAdapterTest<MaxLengthValidatorAdapterNpgsqlTests.TestDbContext>(testOutputHelper)
{
    [StrongType<string>]
    public sealed partial class UserName : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMaxLength(10);
    }

    public sealed class Account
    {
        public required int Id { get; init; }
        public required UserName UserName { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>();
        }
    }


    protected override void ConfigureOptions(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MaxLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task UserName_column_max_length_matches_validator_configuration()
    {
        var maxLength = await Context.Database
            .SqlQuery<int?>(
                $"""
                select character_maximum_length as "Value"
                from information_schema.columns
                where table_name = 'Account' and column_name = 'UserName'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(10, maxLength);
    }
        
    // todo: delete manual implementation once source generators is implemented
    public sealed partial class UserName : IStrongType<UserName, string>
    {
        private readonly string _value;

        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();

        private UserName(string value)
        {
            _value = value;
        }

        public static UserName Create(string value)
        {
            StrongType.EnsureValid(value, Definition);

            return new UserName(value);
        }

        public string AsPrimitive() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object? obj) => obj is UserName other && _value == other._value;
    }
}
