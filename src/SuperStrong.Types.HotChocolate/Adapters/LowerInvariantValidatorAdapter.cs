using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class LowerInvariantValidatorAdapter : StrongTypeValidatorAdapter<LowerInvariantValidator, string, LowerInvariantDirective>
{
    protected override ImmutableArray<LowerInvariantDirective> CreateDirectivesCore(
        IReadOnlyList<LowerInvariantValidator> validators)
    {
        return [new LowerInvariantDirective()];
    }
}
