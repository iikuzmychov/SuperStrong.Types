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

        var primitiveMapping = typeMappingSource.Value.FindMapping(strongTypeInfo.PrimitiveType);

        if (primitiveMapping is null)
        {
            return null;
        }

        return primitiveMapping.Clone(
            clrType: strongTypeInfo.StrongType,
            converter: CreateConverter(strongTypeInfo));
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
