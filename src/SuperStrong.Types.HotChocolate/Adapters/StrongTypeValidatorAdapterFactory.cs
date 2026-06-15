namespace SuperStrong.Types.HotChocolate.Adapters;

public abstract class StrongTypeValidatorAdapterFactory
{
    public abstract bool CanHandle(Type validatorType);

    public abstract IStrongTypeValidatorAdapter Create(Type validatorType);
}
