using HotChocolate.Configuration;
using HotChocolate.Internal;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Configurations;
using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Reflection;
using System.Collections;
using System.Reflection;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeScalarInterceptor(StrongTypeHotChocolateOptions options) : TypeInterceptor
{
    public override void OnBeforeRegisterDependencies(
        ITypeDiscoveryContext discoveryContext,
        TypeSystemConfiguration configuration)
    {
        if (configuration is not ScalarTypeConfiguration scalarConfiguration)
        {
            return;
        }

        if (GetScalarTypeArguments(discoveryContext.Type.GetType()) is not var (runtimeType, valueNodeType))
        {
            return;
        }

        if (runtimeType.GetStrongTypeInfo() is null)
        {
            return;
        }

        var inspector = discoveryContext.TypeInspector;
        var naming = discoveryContext.DescriptorContext.Naming;

        if (GetPrimitiveScalarType(valueNodeType) is { } scalarType)
        {
            var primitive = PrimitiveDirective.From(naming.GetTypeName(scalarType));
            scalarConfiguration.AddDirective(primitive, inspector);
        }

        var groups = GetValidators(runtimeType).GroupBy(validator => validator.GetType());

        foreach (var group in groups)
        {
            if (!options.Adapters.TryGetValue(group.Key, out var build))
            {
                continue;
            }

            foreach (var directive in build(group.ToList()))
            {
                scalarConfiguration.AddDirective(directive, inspector);
            }
        }
    }

    private static (Type RuntimeType, Type ValueNodeType)? GetScalarTypeArguments(Type scalarType)
    {
        for (var current = scalarType; current is not null; current = current.BaseType)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(ScalarType<,>))
            {
                var arguments = current.GetGenericArguments();

                return (arguments[0], arguments[1]);
            }
        }

        return null;
    }

    private static Type? GetPrimitiveScalarType(Type valueNodeType)
    {
        if (valueNodeType == typeof(StringValueNode))
        {
            return typeof(StringType);
        }

        if (valueNodeType == typeof(IntValueNode))
        {
            return typeof(IntType);
        }

        if (valueNodeType == typeof(FloatValueNode))
        {
            return typeof(FloatType);
        }

        if (valueNodeType == typeof(BooleanValueNode))
        {
            return typeof(BooleanType);
        }

        return null;
    }

    private static IEnumerable<object> GetValidators(Type strongType)
    {
        var definitionProperty = strongType.GetProperty(
            nameof(IStrongType<,>.Definition),
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (definitionProperty is null)
        {
            yield break;
        }

        var definition = definitionProperty.GetValue(null);

        if (definition is null)
        {
            yield break;
        }

        var validatorsProperty = definition
            .GetType()
            .GetProperty(nameof(StrongTypeDefinition<>.Validators))!;

        var validators = (IEnumerable)validatorsProperty.GetValue(definition)!;

        foreach (var validator in validators)
        {
            yield return validator!;
        }
    }
}
