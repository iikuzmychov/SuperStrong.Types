namespace SuperStrong.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongTypeAttribute<TPrimitive> : Attribute
    where TPrimitive : notnull;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongTypeAttribute<TPrimitive, TTemplate> : Attribute
    where TPrimitive : notnull
    where TTemplate : notnull, IStrongTypeTemplate<TPrimitive>;
