using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.EntityFrameworkCore.Internal;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.SqlServer.Adapters;

public sealed class SqlServerMinLengthValidatorAdapter : StrongTypeValidatorAdapter<MinLengthValidator, string>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MinLengthValidator> validators,
        IConventionProperty property)
    {
        var minLength = validators.Max(validator => validator.MinLength);

        CheckConstraintRegistrar.TryRegister<string>(
            property,
            purpose: "MinLength",
            buildSql: (columnName, _) => $"LEN([{columnName}]) >= {minLength}");
    }
}
