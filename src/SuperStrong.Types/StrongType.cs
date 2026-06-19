namespace SuperStrong.Types;

public static class StrongType
{
    public static StrongTypeDefinition<TPrimitive> Define<TPrimitive>()
        where TPrimitive : notnull
    {
        return StrongTypeDefinition<TPrimitive>.Empty;
    }

    public static bool IsValid<TPrimitive>(TPrimitive? value, StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : notnull
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (value is null)
        {
            return false;
        }

        foreach (var validator in definition.Validators)
        {
            if (!validator.IsValid(value))
            {
                return false;
            }
        }

        return true;
    }

    public static void EnsureValid<TPrimitive>(TPrimitive value, StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : notnull
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(definition);

        foreach (var validator in definition.Validators)
        {
            validator.EnsureValid(value);
        }
    }

    public static StrongTypeDefinition<TPrimitive> GetDefinition<TStrongType, TPrimitive>()
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        return TStrongType.Definition;
    }

    public static StrongTypeDefinition<TPrimitive> GetTemplateDefinition<TTemplate, TPrimitive>()
        where TTemplate : IStrongTypeTemplate<TPrimitive>
        where TPrimitive : notnull
    {
        return TTemplate.Definition;
    }
}
