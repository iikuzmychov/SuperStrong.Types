using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Numerics;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class MaxValueValidatorAdapter<TPrimitive>
    : StrongTypeValidatorAdapter<MaxValueValidator<TPrimitive>, TPrimitive, MaxValueDirective>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    protected override ImmutableArray<MaxValueDirective> CreateDirectivesCore(
        IReadOnlyList<MaxValueValidator<TPrimitive>> validators)
    {
        return validators
            .Select(validator => new MaxValueDirective
            {
                Value = JsonSerializer.SerializeToElement(validator.MaxValue),
                IsExclusive = validator.IsExclusive,
            })
            .ToImmutableArray();
    }
}
