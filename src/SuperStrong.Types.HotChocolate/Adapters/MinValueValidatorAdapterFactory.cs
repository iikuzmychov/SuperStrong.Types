using SuperStrong.Types.Validators;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class MinValueValidatorAdapterFactory : StrongTypeValidatorAdapterFactory
{
    public override bool CanHandle(Type validatorType)
    {
        ArgumentNullException.ThrowIfNull(validatorType);

        return
            validatorType.IsGenericType &&
            validatorType.GetGenericTypeDefinition() == typeof(MinValueValidator<>);
    }

    public override IStrongTypeValidatorAdapter Create(Type validatorType)
    {
        ArgumentNullException.ThrowIfNull(validatorType);

        var primitiveType = validatorType.GetGenericArguments()[0];
        var adapterType = typeof(MinValueValidatorAdapter<>).MakeGenericType(primitiveType);

        return (IStrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;
    }
}
