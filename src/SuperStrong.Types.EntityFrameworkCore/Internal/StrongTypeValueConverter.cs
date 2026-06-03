using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeValueConverter<TStrongType, TPrimitive> : ValueConverter<TStrongType, TPrimitive>
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    public static StrongTypeValueConverter<TStrongType, TPrimitive> Instance { get; } = new();

    private StrongTypeValueConverter() : base(
        strongType => strongType.AsPrimitive(),
        primitive => CreateStrongType(primitive))
    {
    }

    private static TStrongType CreateStrongType(TPrimitive primitive) => TStrongType.From(primitive);
}