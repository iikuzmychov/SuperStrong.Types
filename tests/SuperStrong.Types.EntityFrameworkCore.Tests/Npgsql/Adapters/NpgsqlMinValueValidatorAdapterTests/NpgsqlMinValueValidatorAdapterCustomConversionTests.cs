using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinValueValidatorAdapterTests;

public sealed partial class NpgsqlMinValueValidatorAdapterCustomConversionTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinValueValidatorAdapterCustomConversionTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Age : IHasStrongTypeDefinition<int>
    {
        public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>().HasMinValue(0);
    }

    public sealed class Person
    {
        public required int Id { get; init; }
        public required Age Age { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Person>()
                .Property(person => person.Age)
                .HasConversion(
                    age => age.AsPrimitive() * 10,
                    stored => Age.From(stored / 10));
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new NpgsqlMinValueValidatorAdapterFactory());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_has_user_supplied_value_converter()
    {
        var checkConstraintCount = await Context.Database
            .SqlQuery<long>(
                $"""
                select count(*) as "Value"
                from pg_constraint constraint_
                join pg_class table_ on constraint_.conrelid = table_.oid
                where table_.relname = 'Person' and constraint_.contype = 'c'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, checkConstraintCount);
    }

}
