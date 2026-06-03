using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

public sealed class MaxValueValidatorAdapterFactory : StrongTypeValidatorAdapterFactory
{
    public override bool CanHandle(Type validatorType)
    {
        return
            validatorType.IsGenericType &&
            validatorType.GetGenericTypeDefinition() == typeof(MaxValueValidator<>);
    }

    public override StrongTypeValidatorAdapter CreateValidatorAdapter(Type validatorType)
    {
        var primitiveType = validatorType.GetGenericArguments()[0];
        var adapterType = typeof(MaxValueValidatorAdapter<>).MakeGenericType(primitiveType);

        return (StrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;
    }
}
