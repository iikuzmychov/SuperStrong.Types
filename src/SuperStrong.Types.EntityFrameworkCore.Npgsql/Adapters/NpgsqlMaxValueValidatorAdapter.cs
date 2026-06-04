using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.EntityFrameworkCore.Internal;
using SuperStrong.Types.Validators;
using System.Numerics;

namespace SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

public sealed class NpgsqlMaxValueValidatorAdapter<TPrimitive> : StrongTypeValidatorAdapter<MaxValueValidator<TPrimitive>, TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MaxValueValidator<TPrimitive>> validators,
        IConventionProperty property)
    {
        var maxValue = validators.Min(validator => validator.MaxValue);

        CheckConstraintRegistrar.TryRegister<TPrimitive>(
            property,
            purpose: "MaxValue",
            buildSql: (columnName, mapping) =>
            {
                var literal = mapping.GenerateProviderValueSqlLiteral(maxValue);
                return $"\"{columnName}\" <= {literal}";
            });
    }
}
