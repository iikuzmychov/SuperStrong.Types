namespace SuperStrong.Types.Validators;

public sealed class PredicateValidator<TPrimitive>(
    Func<TPrimitive, bool> predicate,
    Func<TPrimitive, string?>? errorMessageFactory = null)
    : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public Func<TPrimitive, bool> Predicate { get; } = predicate ?? throw new ArgumentNullException(nameof(predicate));
    public Func<TPrimitive, string?>? ErrorMessageFactory { get; } = errorMessageFactory;

    public override StrongTypeValidatorResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!Predicate(value))
        {
            var errorMessage = ErrorMessageFactory?.Invoke(value) ?? "Value must satisfy the predicate.";

            return StrongTypeValidatorResult.Invalid(errorMessage);
        }

        return StrongTypeValidatorResult.Valid();
    }
}
