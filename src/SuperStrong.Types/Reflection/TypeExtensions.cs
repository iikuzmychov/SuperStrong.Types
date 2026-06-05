using System.Collections.Concurrent;
using System.Reflection;

namespace SuperStrong.Types.Reflection;

public static class TypeExtensions
{
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
            .Where(attribute => attribute.AttributeType.IsGenericType)
            .Where(attribute =>
                attribute.AttributeType.GetGenericTypeDefinition() == typeof(StrongTypeAttribute<>) ||
                attribute.AttributeType.GetGenericTypeDefinition() == typeof(StrongTypeAttribute<,>))
            .ToList();

        if (strongTypeAttributes.Count == 0)
        {
            return null;
        }

        if (strongTypeAttributes.Count > 1)
        {
            throw new InvalidOperationException($"{type} has multiple [{nameof(StrongTypeAttribute<>)}] declarations.");
        }

        var attributeGenericArguments = strongTypeAttributes[0].AttributeType.GetGenericArguments();
        var primitiveType = attributeGenericArguments[0];
        var templateType = attributeGenericArguments.Length == 2 ? attributeGenericArguments[1] : null;
        var hasDefinitionInterface = typeof(IHasStrongTypeDefinition<>).MakeGenericType(primitiveType);

        if (!hasDefinitionInterface.IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"{type} does not implement {hasDefinitionInterface}.");
        }

        var definition = (StrongTypeDefinition)typeof(StrongType)
            .GetMethod(nameof(StrongType.GetDefinition), BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(type, primitiveType)
            .Invoke(null, null)!;

        return new StrongTypeInfo(type, primitiveType, templateType, definition);
    }
}
