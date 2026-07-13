using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types;

public static class StrongType
{
    public static StrongTypeDefinition<TPrimitive> Define<TPrimitive>()
        where TPrimitive : notnull
    {
        return StrongTypeDefinition<TPrimitive>.Empty;
    }

    public static bool IsValid<TStrongType, TPrimitive>([NotNullWhen(true)] TPrimitive? value)
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        if (value is null)
        {
            return false;
        }

        var strongTypeDefinition = GetDefinition<TStrongType, TPrimitive>();

        foreach (var validator in strongTypeDefinition.Validators)
        {
            if (validator.Validate(value) is { IsValid: false })
            {
                return false;
            }
        }

        return true;
    }

    public static void EnsureValid<TStrongType, TPrimitive>(TPrimitive value)
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        ArgumentNullException.ThrowIfNull(value);

        var strongTypeDefinition = GetDefinition<TStrongType, TPrimitive>();
        var errorMessages = ImmutableArray.CreateBuilder<string>(initialCapacity: strongTypeDefinition.Validators.Length);

        foreach (var validator in strongTypeDefinition.Validators)
        {
            if (validator.Validate(value) is { IsValid: false } invalidValidationResult)
            {
                errorMessages.Add(invalidValidationResult.ErrorMessage);
            }
        }

        if (errorMessages.Count > 0)
        {
            throw new StrongTypeValidationException(typeof(TStrongType), value, errorMessages.ToImmutable());
        }
    }

    public static StrongTypeDefinition<TPrimitive> GetDefinition<TStrongType, TPrimitive>()
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        return DefinitionCache<TStrongType, TPrimitive>.Definition;
    }

    public static StrongTypeDefinition<TPrimitive> GetTemplateDefinition<TTemplate, TPrimitive>()
        where TTemplate : IStrongTypeTemplate<TPrimitive>
        where TPrimitive : notnull
    {
        return TemplateDefinitionCache<TTemplate, TPrimitive>.Definition;
    }

    private static class DefinitionCache<TStrongType, TPrimitive>
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        public static readonly StrongTypeDefinition<TPrimitive> Definition = TStrongType.Define();
    }

    private static class TemplateDefinitionCache<TTemplate, TPrimitive>
        where TTemplate : IStrongTypeTemplate<TPrimitive>
        where TPrimitive : notnull
    {
        public static readonly StrongTypeDefinition<TPrimitive> Definition = TTemplate.Define();
    }
}
