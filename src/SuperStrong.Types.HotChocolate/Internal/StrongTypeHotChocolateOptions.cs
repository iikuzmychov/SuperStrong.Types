using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeHotChocolateOptions
{
    private readonly Dictionary<Type, Func<IReadOnlyList<object>, ImmutableArray<object>>> _adapters = [];
    private readonly HashSet<Type> _directiveTypes = [];

    public IReadOnlyDictionary<Type, Func<IReadOnlyList<object>, ImmutableArray<object>>> Adapters => _adapters;
    public IReadOnlySet<Type> DirectiveTypes => _directiveTypes;

    public void RegisterAdapter<TValidator, TPrimitive, TDirective>(
        IStrongTypeValidatorDirectiveAdapter<TValidator, TPrimitive, TDirective> adapter)
        where TValidator : StrongTypeValidator<TPrimitive>
        where TPrimitive : notnull
    {
        ArgumentNullException.ThrowIfNull(adapter);

        _adapters[typeof(TValidator)] = validators =>
        {
            var typedList = validators.Cast<TValidator>().ToList();
            var directives = adapter.CreateDirectives(typedList);

            var builder = ImmutableArray.CreateBuilder<object>(directives.Length);

            foreach (var directive in directives)
            {
                builder.Add(directive!);
            }

            return builder.ToImmutable();
        };
    }

    public void RegisterDirectiveType(Type directiveType)
    {
        ArgumentNullException.ThrowIfNull(directiveType);

        _directiveTypes.Add(directiveType);
    }
}
