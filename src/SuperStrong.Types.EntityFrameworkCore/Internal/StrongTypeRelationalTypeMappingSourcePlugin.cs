using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeRelationalTypeMappingSourcePlugin(Lazy<IRelationalTypeMappingSource> typeMappingSource)
    : IRelationalTypeMappingSourcePlugin
{
    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType?.GetStrongTypeInfo() is not { } strongTypeInfo)
        {
            return null;
        }

        var primitiveMapping = typeMappingSource.Value.FindMapping(
            type: strongTypeInfo.PrimitiveType,
            storeTypeName: mappingInfo.StoreTypeName,
            keyOrIndex: mappingInfo.IsKeyOrIndex,
            unicode: mappingInfo.IsUnicode,
            size: mappingInfo.Size,
            rowVersion: mappingInfo.IsRowVersion,
            fixedLength: mappingInfo.IsFixedLength,
            precision: mappingInfo.Precision,
            scale: mappingInfo.Scale);

        if (primitiveMapping is null)
        {
            return null;
        }

        var converter = CreateConverter(strongTypeInfo);

        return (RelationalTypeMapping)primitiveMapping.WithComposedConverter(converter);
    }

    private static ValueConverter CreateConverter(StrongTypeInfo strongTypeInfo)
    {
        var converterType = typeof(StrongTypeValueConverter<,>)
            .MakeGenericType(strongTypeInfo.StrongType, strongTypeInfo.PrimitiveType);

        return (ValueConverter)converterType
            .GetProperty(nameof(StrongTypeValueConverter<,>.Instance))!
            .GetValue(null)!;
    }
}
