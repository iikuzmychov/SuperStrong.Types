using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Adapters;

public abstract class StrongTypeValidatorAdapter<TValidator, TPrimitive, TDirective>
    : IStrongTypeValidatorDirectiveAdapter<TValidator, TPrimitive, TDirective>
    where TValidator : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public ImmutableArray<TDirective> CreateDirectives(IReadOnlyList<TValidator> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);

        if (validators.Count == 0)
        {
            return [];
        }

        return CreateDirectivesCore(validators);
    }

    protected abstract ImmutableArray<TDirective> CreateDirectivesCore(IReadOnlyList<TValidator> validators);
}
