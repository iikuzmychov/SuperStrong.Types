using System.Collections.Immutable;
using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class MinLengthValidatorAdapter : StrongTypeValidatorAdapter<MinLengthValidator, string, MinLengthDirective>
{
    protected override ImmutableArray<MinLengthDirective> CreateDirectivesCore(
        IReadOnlyList<MinLengthValidator> validators)
    {
        var minLength = validators.Max(validator => validator.MinLength);

        return [new MinLengthDirective { Value = minLength }];
    }
}
