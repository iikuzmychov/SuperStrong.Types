using System.Collections.Concurrent;
using System.Reflection;

namespace SuperStrong.Types.Reflection;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, StrongTypeInfo?> _strongTypeInfoCache = new();

    public static StrongTypeInfo? GetStrongTypeInfo(this Type strongType)
    {
        ArgumentNullException.ThrowIfNull(strongType);

        return _strongTypeInfoCache.GetOrAdd(strongType, GetStrongTypeInfoCore);
    }

    private static StrongTypeInfo? GetStrongTypeInfoCore(Type strongType)
    {
        var strongTypeAttributes = strongType
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
            throw new InvalidOperationException($"{strongType} has multiple [{nameof(StrongTypeAttribute<>)}] declarations.");
        }

        var attributeGenericArguments = strongTypeAttributes[0].AttributeType.GetGenericArguments();
        var primitiveType = attributeGenericArguments[0];
        var templateType = attributeGenericArguments.Length == 2 ? attributeGenericArguments[1] : null;
        var hasDefinitionInterface = typeof(IHasStrongTypeDefinition<>).MakeGenericType(primitiveType);

        if (!hasDefinitionInterface.IsAssignableFrom(strongType))
        {
            throw new InvalidOperationException($"{strongType} does not implement {hasDefinitionInterface}.");
        }

        var definition = (StrongTypeDefinition)typeof(TypeExtensions)
            .GetMethod(nameof(GetDefinition), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(strongType, primitiveType)
            .Invoke(null, null)!;

        return new StrongTypeInfo(strongType, primitiveType, templateType, definition);
    }

    private static StrongTypeDefinition<TPrimitive> GetDefinition<TStrongType, TPrimitive>()
        where TStrongType : IHasStrongTypeDefinition<TPrimitive>
        where TPrimitive : notnull
    {
        return TStrongType.Definition;
    }
}
