namespace SuperStrong.Types;

public interface IStrongTypeTemplate<TPrimitive> : IHasStrongTypeDefinition<TPrimitive>, IHasStrongTypeLayout<TPrimitive>
    where TPrimitive : notnull;
