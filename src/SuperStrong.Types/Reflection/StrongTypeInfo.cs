namespace SuperStrong.Types.Reflection;

public sealed class StrongTypeInfo
{
    public Type ClrType { get; }
    public Type PrimitiveType { get; }
    public Type? TemplateType { get; }
    public StrongTypeDefinition Definition { get; }

    internal StrongTypeInfo(Type clrType, Type primitiveType, Type? templateType, StrongTypeDefinition definition)
    {
        ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        PrimitiveType = primitiveType ?? throw new ArgumentNullException(nameof(primitiveType));
        TemplateType = templateType;
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }
}
