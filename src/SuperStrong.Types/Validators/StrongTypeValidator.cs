namespace SuperStrong.Types.Validators;

public abstract class StrongTypeValidator
{
    private protected StrongTypeValidator()
    {
    }
}

public abstract class StrongTypeValidator<TPrimitive> : StrongTypeValidator
    where TPrimitive : notnull
{
    public bool IsValid(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return GetValidationException(value) is null;
    }

    public void EnsureValid(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var exception = GetValidationException(value);

        if (exception is not null)
        {
            throw exception;
        }
    }

    protected abstract Exception? GetValidationException(TPrimitive value);
}
