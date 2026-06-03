using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.Validators;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore.Adapters;

public abstract class StrongTypeValidatorAdapter
{
    private protected StrongTypeValidatorAdapter()
    {
    }

    public abstract bool CanHandle(Type validatorType);

    internal abstract void Apply(
        Type validatorType,
        IReadOnlyList<StrongTypeValidator> validators,
        IConventionProperty property,
        Type strongType);
}

public abstract class StrongTypeValidatorAdapter<TValidator, TPrimitive> : StrongTypeValidatorAdapter
    where TValidator : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    private delegate void Dispatcher(
        StrongTypeValidatorAdapter<TValidator, TPrimitive> instance,
        IReadOnlyList<TValidator> validators,
        IConventionProperty property);

    private static readonly ConcurrentDictionary<Type, Dispatcher> _dispatchers = new();

    public sealed override bool CanHandle(Type validatorType) => validatorType == typeof(TValidator);

    public void Apply<TStrongType>(IReadOnlyList<TValidator> validators, IConventionProperty property)
        where TStrongType : IStrongType<TStrongType, TPrimitive>
    {
        ArgumentNullException.ThrowIfNull(validators);
        ArgumentNullException.ThrowIfNull(property);

        if (validators.Count == 0)
        {
            return;
        }

        ApplyCore<TStrongType>(validators, property);
    }

    internal sealed override void Apply(
        Type validatorType,
        IReadOnlyList<StrongTypeValidator> validators,
        IConventionProperty property,
        Type strongType)
    {
        if (validators.Count == 0)
        {
            return;
        }

        var dispatch = _dispatchers.GetOrAdd(strongType, CreateDispatcher);
        dispatch(this, validators.Cast<TValidator>().ToImmutableArray(), property);
    }

    protected abstract void ApplyCore<TStrongType>(IReadOnlyList<TValidator> validators, IConventionProperty property)
        where TStrongType : IStrongType<TStrongType, TPrimitive>;

    private static Dispatcher CreateDispatcher(Type strongType)
    {
        var openApplyMethod = typeof(StrongTypeValidatorAdapter<TValidator, TPrimitive>)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(method => method.Name == nameof(Apply) && method.IsGenericMethodDefinition);

        var closedApplyMethod = openApplyMethod.MakeGenericMethod(strongType);

        return (instance, validators, property) => closedApplyMethod.Invoke(instance, [validators, property]);
    }
}
