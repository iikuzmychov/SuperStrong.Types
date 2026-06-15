using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class ForbiddenValuesValidatorAdapter<TPrimitive>
    : StrongTypeValidatorAdapter<ForbiddenValuesValidator<TPrimitive>, TPrimitive, ForbiddenValuesDirective>
    where TPrimitive : notnull
{
    protected override ImmutableArray<ForbiddenValuesDirective> CreateDirectivesCore(
        IReadOnlyList<ForbiddenValuesValidator<TPrimitive>> validators)
    {
        return validators
            .Select(validator => new ForbiddenValuesDirective
            {
                Values = validator.ForbiddenValues
                    .OrderBy(value => PrimitiveFormatter.Format(value), StringComparer.Ordinal)
                    .Select(value => (object)JsonSerializer.SerializeToElement(value))
                    .ToImmutableArray(),
            })
            .ToImmutableArray();
    }
}
