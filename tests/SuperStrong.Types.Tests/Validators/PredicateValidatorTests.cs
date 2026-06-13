using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class PredicateValidatorTests
{
    [Fact]
    public void Constructor_throws_for_null_predicate()
    {
        Assert.Throws<ArgumentNullException>(() => new PredicateValidator<int>(null!));
    }

    [Fact]
    public void Exposes_predicate()
    {
        Func<int, bool> predicate = value => value > 0;

        var validator = new PredicateValidator<int>(predicate);

        Assert.Same(predicate, validator.Predicate);
    }

    [Fact]
    public void IsValid_returns_true_when_predicate_is_satisfied()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.True(validator.IsValid(1));
    }

    [Fact]
    public void IsValid_returns_false_when_predicate_is_not_satisfied()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.False(validator.IsValid(0));
    }

    [Fact]
    public void EnsureValid_throws_when_predicate_is_not_satisfied()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.Throws<ArgumentException>(() => validator.EnsureValid(0));
    }
}
