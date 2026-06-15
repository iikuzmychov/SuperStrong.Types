using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class AllowedValuesValidatorAdapter<TPrimitive>
    : StrongTypeValidatorAdapter<AllowedValuesValidator<TPrimitive>, TPrimitive, AllowedValuesDirective>
    where TPrimitive : notnull
{
    protected override ImmutableArray<AllowedValuesDirective> CreateDirectivesCore(
        IReadOnlyList<AllowedValuesValidator<TPrimitive>> validators)
    {
        return validators
            .Select(validator => new AllowedValuesDirective
            {
                Values = validator.AllowedValues
                    .OrderBy(value => PrimitiveFormatter.Format(value), StringComparer.Ordinal)
                    .Select(value => (object)JsonSerializer.SerializeToElement(value))
                    .ToImmutableArray(),
            })
            .ToImmutableArray();
    }
}
