using System.Collections.Immutable;
using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class MaxLengthValidatorAdapter : StrongTypeValidatorAdapter<MaxLengthValidator, string, MaxLengthDirective>
{
    protected override ImmutableArray<MaxLengthDirective> CreateDirectivesCore(
        IReadOnlyList<MaxLengthValidator> validators)
    {
        var maxLength = validators.Min(validator => validator.MaxLength);

        return [new MaxLengthDirective { Value = maxLength }];
    }
}
