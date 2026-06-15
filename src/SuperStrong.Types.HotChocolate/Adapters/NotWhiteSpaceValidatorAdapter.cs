using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class NotWhiteSpaceValidatorAdapter : StrongTypeValidatorAdapter<NotWhiteSpaceValidator, string, NotWhiteSpaceDirective>
{
    protected override ImmutableArray<NotWhiteSpaceDirective> CreateDirectivesCore(
        IReadOnlyList<NotWhiteSpaceValidator> validators)
    {
        return [new NotWhiteSpaceDirective()];
    }
}
