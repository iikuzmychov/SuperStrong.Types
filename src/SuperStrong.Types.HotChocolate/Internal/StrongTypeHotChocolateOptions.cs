using SuperStrong.Types.HotChocolate.Adapters;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeHotChocolateOptions
{
    private readonly Dictionary<Type, Func<IReadOnlyList<object>, ImmutableArray<object>>> _adapters = [];
    private readonly List<StrongTypeValidatorAdapterFactory> _adapterFactories = [];
    private readonly HashSet<Type> _directiveTypes = [];

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

    public void RegisterAdapterFactory(StrongTypeValidatorAdapterFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _adapterFactories.Add(factory);
    }

    public void RegisterDirectiveType(Type directiveType)
    {
        ArgumentNullException.ThrowIfNull(directiveType);

        _directiveTypes.Add(directiveType);
    }

    public Func<IReadOnlyList<object>, ImmutableArray<object>>? ResolveAdapter(Type validatorType)
    {
        if (_adapters.TryGetValue(validatorType, out var adapter))
        {
            return adapter;
        }

        foreach (var factory in _adapterFactories)
        {
            if (!factory.CanHandle(validatorType))
            {
                continue;
            }

            var created = factory.Create(validatorType);

            ImmutableArray<object> build(IReadOnlyList<object> validators)
            {
                return created.CreateDirectives(validators);
            }

            _adapters[validatorType] = build;

            return build;
        }

        return null;
    }
}
