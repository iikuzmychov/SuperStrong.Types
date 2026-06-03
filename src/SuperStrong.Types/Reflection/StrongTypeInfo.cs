namespace SuperStrong.Types.Reflection;

public sealed class StrongTypeInfo
{
    public Type StrongType { get; }
    public Type PrimitiveType { get; }
    public Type? TemplateType { get; }
    public StrongTypeDefinition Definition { get; }

    internal StrongTypeInfo(Type strongType, Type primitiveType, Type? templateType, StrongTypeDefinition definition)
    {
        StrongType = strongType ?? throw new ArgumentNullException(nameof(strongType));
        PrimitiveType = primitiveType ?? throw new ArgumentNullException(nameof(primitiveType));
        TemplateType = templateType;
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }
}
