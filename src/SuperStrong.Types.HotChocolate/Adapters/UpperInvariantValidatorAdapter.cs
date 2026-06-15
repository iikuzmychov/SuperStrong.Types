using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class UpperInvariantValidatorAdapter : StrongTypeValidatorAdapter<UpperInvariantValidator, string, UpperInvariantDirective>
{
    protected override ImmutableArray<UpperInvariantDirective> CreateDirectivesCore(
        IReadOnlyList<UpperInvariantValidator> validators)
    {
        return [new UpperInvariantDirective()];
    }
}
