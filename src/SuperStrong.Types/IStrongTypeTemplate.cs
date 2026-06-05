namespace SuperStrong.Types;

public interface IStrongTypeTemplate<TPrimitive> : IHasStrongTypeDefinition<TPrimitive>
    where TPrimitive : notnull;
