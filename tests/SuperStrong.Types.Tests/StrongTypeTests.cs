namespace SuperStrong.Types.Tests;

public sealed partial class StrongTypeTests
{
    [StrongType<string>]
    private sealed partial class ValidatedLowerCaseString
    {
        public static StrongTypeDefinition<string> Define() => StrongType
            .Define<string>()
            .HasMinLength(3)
            .IsLowerInvariant();
    }

    [Fact]
    public void IsValid_returns_true_when_all_validators_pass()
    {
        Assert.True(StrongType.IsValid<ValidatedLowerCaseString, string>("abc"));
    }

    [Fact]
    public void IsValid_returns_false_when_any_validator_fails()
    {
        Assert.False(StrongType.IsValid<ValidatedLowerCaseString, string>("ABC"));
    }

    [Fact]
    public void IsValid_returns_false_for_null()
    {
        Assert.False(StrongType.IsValid<ValidatedLowerCaseString, string>(null));
    }

    [Fact]
    public void EnsureValid_returns_when_all_validators_pass()
    {
        StrongType.EnsureValid<ValidatedLowerCaseString, string>("abc");
    }

    [Fact]
    public void EnsureValid_throws_StrongTypeValidationException_when_a_validator_fails()
    {
        var exception = Assert.Throws<StrongTypeValidationException>(
            () => StrongType.EnsureValid<ValidatedLowerCaseString, string>("ab"));

        Assert.Equal(typeof(ValidatedLowerCaseString), exception.StrongType);
        Assert.Equal("ab", exception.Value);
        Assert.Equal("Value must be at least 3 characters long.", Assert.Single(exception.ErrorMessages));

        Assert.Equal(
            $"Validation failed for strong type '{typeof(ValidatedLowerCaseString)}'. Errors:{Environment.NewLine}" +
            "- Value must be at least 3 characters long.",
            exception.Message);
    }

    [Fact]
    public void EnsureValid_aggregates_all_failing_validators()
    {
        var exception = Assert.Throws<StrongTypeValidationException>(
            () => StrongType.EnsureValid<ValidatedLowerCaseString, string>("AB"));

        Assert.Equal(
            ["Value must be at least 3 characters long.", "Value must be in lower invariant case."],
            exception.ErrorMessages);

        Assert.Equal(
            $"Validation failed for strong type '{typeof(ValidatedLowerCaseString)}'. Errors:{Environment.NewLine}" +
            $"- Value must be at least 3 characters long.{Environment.NewLine}" +
            "- Value must be in lower invariant case.",
            exception.Message);
    }

    [Fact]
    public void EnsureValid_throws_for_null_value()
    {
        Assert.Throws<ArgumentNullException>(() => StrongType.EnsureValid<ValidatedLowerCaseString, string>(null!));
    }
}
