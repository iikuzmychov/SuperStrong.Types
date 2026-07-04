using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.Converters;

public sealed class StrongTypeConverter : TypeConverter
{
    private readonly TypeConverter _converter;

    public StrongTypeConverter(Type strongType)
    {
        ArgumentNullException.ThrowIfNull(strongType);

        var strongTypeInfo = strongType.GetStrongTypeInfo()
            ?? throw new ArgumentException($"{strongType} is not a strong type.", nameof(strongType));

        var converterType = typeof(StrongTypeConverter<,>)
            .MakeGenericType(strongTypeInfo.ClrType, strongTypeInfo.PrimitiveType);

        _converter = (TypeConverter)Activator.CreateInstance(converterType)!;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return _converter.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return _converter.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        return _converter.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return _converter.ConvertTo(context, culture, value, destinationType);
    }
}

public sealed class StrongTypeConverter<TStrongType, TPrimitive> : TypeConverter
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    private static readonly TypeConverter _primitiveConverter = TypeDescriptor.GetConverter(typeof(TPrimitive));

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return
            sourceType == typeof(TPrimitive) ||
            _primitiveConverter.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            TPrimitive primitive => TStrongType.From(primitive),
            _ when _primitiveConverter.ConvertFrom(context, culture, value) is TPrimitive primitive => TStrongType.From(primitive),
            _ => base.ConvertFrom(context, culture, value),
        };
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        return
            destinationType == typeof(TPrimitive) ||
            _primitiveConverter.CanConvertTo(context, destinationType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value switch
        {
            TStrongType strongType when destinationType == typeof(TPrimitive) => strongType.AsPrimitive(),
            TStrongType strongType => _primitiveConverter.ConvertTo(context, culture, strongType.AsPrimitive(), destinationType),
            _ => base.ConvertTo(context, culture, value, destinationType),
        };
    }
}
