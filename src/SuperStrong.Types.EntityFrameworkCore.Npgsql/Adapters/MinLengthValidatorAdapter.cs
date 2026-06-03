using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.EntityFrameworkCore.Internal;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

public sealed class MinLengthValidatorAdapter : StrongTypeValidatorAdapter<MinLengthValidator, string>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MinLengthValidator> validators,
        IConventionProperty property)
    {
        var minLength = validators.Max(validator => validator.MinLength);

        CheckConstraintRegistrar.TryRegister<string>(
            property,
            purpose: "MinLength",
            buildSql: (columnName, _) => $"LENGTH(\"{columnName}\") >= {minLength}");
    }
}
