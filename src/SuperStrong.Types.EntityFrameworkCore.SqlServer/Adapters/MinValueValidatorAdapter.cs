using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.EntityFrameworkCore.Internal;
using SuperStrong.Types.Validators;
using System.Numerics;

namespace SuperStrong.Types.EntityFrameworkCore.SqlServer.Adapters;

public sealed class MinValueValidatorAdapter<TPrimitive>
    : StrongTypeValidatorAdapter<MinValueValidator<TPrimitive>, TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MinValueValidator<TPrimitive>> validators,
        IConventionProperty property)
    {
        var minValue = validators.Max(validator => validator.MinValue);
        var modelValue = TStrongType.Create(minValue);

        CheckConstraintRegistrar.TryRegister<TPrimitive>(
            property,
            purpose: "MinValue",
            buildSql: (columnName, mapping) =>
            {
                var literal = mapping.GenerateSqlLiteral(modelValue);
                return $"[{columnName}] >= {literal}";
            });
    }
}
