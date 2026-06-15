using SuperStrong.Types.Validators;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class ForbiddenValuesValidatorAdapterFactory : StrongTypeValidatorAdapterFactory
{
    public override bool CanHandle(Type validatorType)
    {
        ArgumentNullException.ThrowIfNull(validatorType);

        return
            validatorType.IsGenericType &&
            validatorType.GetGenericTypeDefinition() == typeof(ForbiddenValuesValidator<>);
    }

    public override IStrongTypeValidatorAdapter Create(Type validatorType)
    {
        ArgumentNullException.ThrowIfNull(validatorType);

        var primitiveType = validatorType.GetGenericArguments()[0];
        var adapterType = typeof(ForbiddenValuesValidatorAdapter<>).MakeGenericType(primitiveType);

        return (IStrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;
    }
}
