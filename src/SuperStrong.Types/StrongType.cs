namespace SuperStrong.Types;

public static class StrongType
{
    public static StrongTypeDefinition<TPrimitive> Define<TPrimitive>()
        where TPrimitive : notnull
    {
        return StrongTypeDefinition<TPrimitive>.Empty;
    }

    public static bool IsValid<TPrimitive>(TPrimitive value, StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : notnull
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(definition);

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

        // todo: maybe return AggregateException with all validation errors instead of throwing on the first failure?
        foreach (var validator in definition.Validators)
        {
            validator.EnsureValid(value);
        }
    }
}
