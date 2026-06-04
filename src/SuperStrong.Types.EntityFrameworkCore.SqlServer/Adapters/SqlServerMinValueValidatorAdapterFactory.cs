using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.SqlServer.Adapters;

public sealed class SqlServerMinValueValidatorAdapterFactory : StrongTypeValidatorAdapterFactory
{
    public override bool CanHandle(Type validatorType)
    {
        return
            validatorType.IsGenericType &&
            validatorType.GetGenericTypeDefinition() == typeof(MinValueValidator<>);
    }

    public override StrongTypeValidatorAdapter CreateValidatorAdapter(Type validatorType)
    {
        var primitiveType = validatorType.GetGenericArguments()[0];
        var adapterType = typeof(SqlServerMinValueValidatorAdapter<>).MakeGenericType(primitiveType);

        return (StrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;
    }
}
