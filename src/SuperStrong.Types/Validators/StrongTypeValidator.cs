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
    public abstract StrongTypeValidatorResult Validate(TPrimitive value);
}
