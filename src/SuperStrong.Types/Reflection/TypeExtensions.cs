using System.Collections.Concurrent;
using System.Reflection;

namespace SuperStrong.Types.Reflection;

public static class TypeExtensions
{
    private const string StrongTypesNamespace = "SuperStrong.Types";
    private const string StrongTypeAttributeName = "StrongTypeAttribute";

    private static readonly ConcurrentDictionary<Type, StrongTypeInfo?> _strongTypeInfoCache = new();

    public static StrongTypeInfo? GetStrongTypeInfo(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return _strongTypeInfoCache.GetOrAdd(type, GetStrongTypeInfoCore);
    }

    private static StrongTypeInfo? GetStrongTypeInfoCore(Type type)
    {
        var strongTypeAttributes = type
            .CustomAttributes
            .Where(IsStrongTypeAttribute)
            .ToList();

        if (strongTypeAttributes.Count == 0)
        {
            return null;
        }

        if (strongTypeAttributes.Count > 1)
        {
            throw new InvalidOperationException($"{type} has multiple [{StrongTypeAttributeName}] declarations.");
        }

        var attributeGenericArguments = strongTypeAttributes[0].AttributeType.GetGenericArguments();
        var primitiveType = attributeGenericArguments[0];
        var templateType = attributeGenericArguments.Length == 2 ? attributeGenericArguments[1] : null;
        var strongTypeInterface = typeof(IStrongType<,>).MakeGenericType(type, primitiveType);

        if (!strongTypeInterface.IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"{type} does not implement {strongTypeInterface}.");
        }

        var definition = (StrongTypeDefinition)typeof(StrongType)
            .GetMethod(nameof(StrongType.GetDefinition), BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(type, primitiveType)
            .Invoke(null, null)!;

        return new StrongTypeInfo(type, primitiveType, templateType, definition);
    }

    private static bool IsStrongTypeAttribute(CustomAttributeData attribute)
    {
        if (!attribute.AttributeType.IsGenericType)
        {
            return false;
        }

        var definition = attribute.AttributeType.GetGenericTypeDefinition();

        return
            definition.Namespace == StrongTypesNamespace &&
            definition.Name is $"{StrongTypeAttributeName}`1" or $"{StrongTypeAttributeName}`2";
    }
}
