using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate;

public interface IStrongTypeValidatorDirectiveAdapter<TValidator, TPrimitive, TDirective>
    where TValidator : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public ImmutableArray<TDirective> CreateDirectives(IReadOnlyList<TValidator> validators);
}
