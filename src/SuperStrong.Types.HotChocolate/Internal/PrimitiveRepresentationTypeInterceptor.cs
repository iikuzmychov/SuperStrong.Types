using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Configurations;
using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class PrimitiveRepresentationTypeInterceptor : TypeInterceptor
{
    public override void OnBeforeRegisterDependencies(
        ITypeDiscoveryContext discoveryContext,
        TypeSystemConfiguration configuration)
    {
        var typeInspector = discoveryContext.TypeInspector;

        switch (configuration)
        {
            case ObjectTypeConfiguration objectType:
                RewriteOutputFields(typeInspector, objectType.Fields);
                break;

            case InterfaceTypeConfiguration interfaceType:
                RewriteOutputFields(typeInspector, interfaceType.Fields);
                break;

            case InputObjectTypeConfiguration inputObjectType:
                foreach (var field in inputObjectType.Fields)
                {
                    RewriteToPrimitive(typeInspector, field);
                }

                break;

            case DirectiveTypeConfiguration directiveType:
                foreach (var argument in directiveType.Arguments)
                {
                    RewriteToPrimitive(typeInspector, argument);
                }

                break;
        }
    }

    private static void RewriteOutputFields(ITypeInspector typeInspector, IEnumerable<OutputFieldConfiguration> fields)
    {
        foreach (var field in fields)
        {
            RewriteToPrimitive(typeInspector, field);

            foreach (var argument in field.Arguments)
            {
                RewriteToPrimitive(typeInspector, argument);
            }
        }
    }

    private static void RewriteToPrimitive(ITypeInspector typeInspector, FieldConfiguration field)
    {
        if (field.Type is not ExtendedTypeReference typeReference)
        {
            return;
        }

        var substituted = SubstituteStrongTypes(typeReference.Type.Source);

        if (substituted == typeReference.Type.Source)
        {
            return;
        }

        var nullability = typeInspector.CollectNullability(typeReference.Type);
        field.Type = typeReference.With(typeInspector.GetType(substituted, nullability));
    }

    private static Type SubstituteStrongTypes(Type type)
    {
        if (type.GetStrongTypeInfo() is { } info)
        {
            return info.PrimitiveType;
        }

        if (type.IsArray && type.GetElementType() is { } elementType)
        {
            var substitutedElement = SubstituteStrongTypes(elementType);

            return substitutedElement == elementType ? type : substitutedElement.MakeArrayType();
        }

        if (type.IsGenericType)
        {
            var arguments = type.GetGenericArguments();
            var changed = false;

            for (var i = 0; i < arguments.Length; i++)
            {
                var substituted = SubstituteStrongTypes(arguments[i]);
                changed |= substituted != arguments[i];
                arguments[i] = substituted;
            }

            return changed ? type.GetGenericTypeDefinition().MakeGenericType(arguments) : type;
        }

        return type;
    }
}
