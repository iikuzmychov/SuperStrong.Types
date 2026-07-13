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
    public void Exposes_error_message_factory()
    {
        Func<int, string?> errorMessageFactory = value => $"Bad value {value}.";

        var validator = new PredicateValidator<int>(value => value > 0, errorMessageFactory);

        Assert.Same(errorMessageFactory, validator.ErrorMessageFactory);
    }

    [Fact]
    public void Error_message_factory_defaults_to_null()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.Null(validator.ErrorMessageFactory);
    }

    [Fact]
    public void Validate_returns_Valid_when_predicate_is_satisfied()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.True(validator.Validate(1).IsValid);
    }

    [Fact]
    public void Validate_returns_Invalid_when_predicate_is_not_satisfied()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        Assert.False(validator.Validate(0).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_a_default_error_message_when_no_factory_is_given()
    {
        var validator = new PredicateValidator<int>(value => value > 0);

        var result = validator.Validate(0);

        Assert.False(result.IsValid);
        Assert.Equal("Value must satisfy the predicate.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_the_factory_error_message()
    {
        var validator = new PredicateValidator<int>(value => value > 0, value => $"Value {value} is not positive.");

        var result = validator.Validate(-1);

        Assert.False(result.IsValid);
        Assert.Equal("Value -1 is not positive.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_falls_back_to_the_default_error_message_when_the_factory_returns_null()
    {
        var validator = new PredicateValidator<int>(value => value > 0, _ => null);

        var result = validator.Validate(0);

        Assert.False(result.IsValid);
        Assert.Equal("Value must satisfy the predicate.", result.ErrorMessage);
    }
}
