using HotChocolate.Internal;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using SuperStrong.Types.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeDiscoveryHandler(ITypeInspector typeInspector) : TypeDiscoveryHandler
{
    public override bool TryInferType(
        TypeReference typeReference,
        TypeDiscoveryInfo typeInfo,
        [NotNullWhen(true)] out TypeReference[]? schemaTypeRefs)
    {
        if (typeInfo.RuntimeType.GetStrongTypeInfo() is not { } info)
        {
            schemaTypeRefs = null;
            return false;
        }

        var scalarType = typeof(StrongScalarType<,>).MakeGenericType(info.ClrType, info.PrimitiveType);
        schemaTypeRefs = [typeInspector.GetTypeRef(scalarType)];

        return true;
    }

    public override bool TryInferKind(
        TypeReference typeReference,
        TypeDiscoveryInfo typeInfo,
        out TypeKind typeKind)
    {
        if (typeInfo.RuntimeType.GetStrongTypeInfo() is null)
        {
            typeKind = default;
            return false;
        }

        typeKind = TypeKind.Scalar;
        return true;
    }
}
