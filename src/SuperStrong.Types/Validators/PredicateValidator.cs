namespace SuperStrong.Types.Validators;

public sealed class PredicateValidator<TPrimitive>(Func<TPrimitive, bool> predicate) : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public Func<TPrimitive, bool> Predicate { get; } = predicate ?? throw new ArgumentNullException(nameof(predicate));

    protected override Exception? GetValidationException(TPrimitive value)
    {
        if (!Predicate(value))
        {
            return new ArgumentException("Value does not satisfy the required condition.", nameof(value));
        }

        return null;
    }
}
