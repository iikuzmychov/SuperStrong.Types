using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types;

public abstract class StrongTypeDefinition
{
    public ImmutableArray<StrongTypeValidator> Validators { get; }

    private protected StrongTypeDefinition(ImmutableArray<StrongTypeValidator> validators)
    {
        Validators = validators;
    }
}

public sealed class StrongTypeDefinition<TPrimitive> : StrongTypeDefinition
    where TPrimitive : notnull
{
    internal static StrongTypeDefinition<TPrimitive> Empty { get; } = new([]);

    public new ImmutableArray<StrongTypeValidator<TPrimitive>> Validators { get; }

    private StrongTypeDefinition(ImmutableArray<StrongTypeValidator<TPrimitive>> validators)
        : base(validators.CastArray<StrongTypeValidator>())
    {
        Validators = validators;
    }

    public StrongTypeDefinition<TPrimitive> WithValidator(StrongTypeValidator<TPrimitive> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);

        return new(Validators.Add(validator));
    }
}
