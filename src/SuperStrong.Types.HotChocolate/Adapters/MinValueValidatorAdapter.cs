using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Numerics;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class MinValueValidatorAdapter<TPrimitive>
    : StrongTypeValidatorAdapter<MinValueValidator<TPrimitive>, TPrimitive, MinValueDirective>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    protected override ImmutableArray<MinValueDirective> CreateDirectivesCore(
        IReadOnlyList<MinValueValidator<TPrimitive>> validators)
    {
        return validators
            .Select(validator => new MinValueDirective
            {
                Value = JsonSerializer.SerializeToElement(validator.MinValue),
                IsExclusive = validator.IsExclusive,
            })
            .ToImmutableArray();
    }
}
