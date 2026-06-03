using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeValueConverterSelector(IValueConverterSelector inner) : IValueConverterSelector
{
    public IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        ArgumentNullException.ThrowIfNull(modelClrType);

        if (modelClrType.GetStrongTypeInfo() is { } strongTypeInfo &&
            (providerClrType is null || providerClrType == strongTypeInfo.PrimitiveType))
        {
            var converterType = typeof(StrongTypeValueConverter<,>)
                .MakeGenericType(strongTypeInfo.StrongType, strongTypeInfo.PrimitiveType);

            yield return new ValueConverterInfo(
                modelClrType: strongTypeInfo.StrongType,
                providerClrType: strongTypeInfo.PrimitiveType,
                factory: _ => (ValueConverter)converterType
                    .GetProperty(nameof(StrongTypeValueConverter<,>.Instance))!
                    .GetValue(null)!);
        }

        foreach (var item in inner.Select(modelClrType, providerClrType))
        {
            yield return item;
        }
    }
}
