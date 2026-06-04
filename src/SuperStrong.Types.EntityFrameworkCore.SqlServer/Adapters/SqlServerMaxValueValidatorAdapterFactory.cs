using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.SqlServer.Adapters;

public sealed class SqlServerMaxValueValidatorAdapterFactory : StrongTypeValidatorAdapterFactory
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
        var adapterType = typeof(SqlServerMaxValueValidatorAdapter<>).MakeGenericType(primitiveType);

        return (StrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;
    }
}
