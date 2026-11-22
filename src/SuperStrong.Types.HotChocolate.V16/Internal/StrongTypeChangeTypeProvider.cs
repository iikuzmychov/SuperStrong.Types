using HotChocolate.Utilities;
using SuperStrong.Types.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeChangeTypeProvider : IChangeTypeProvider
{
    public bool TryCreateConverter(
        Type source,
        Type target,
        ChangeTypeProvider root,
        [NotNullWhen(true)] out ChangeType? converter)
    {
        if (source.GetStrongTypeInfo() is { } sourceInfo)
        {
            var toPrimitive = GetConverter(sourceInfo, nameof(Converters<,>.ToPrimitive));

            if (target == sourceInfo.PrimitiveType)
            {
                converter = toPrimitive;
                return true;
            }

            if (root(sourceInfo.PrimitiveType, target, out var convertPrimitive))
            {
                converter = value => convertPrimitive(toPrimitive(value));
                return true;
            }
        }

        if (target.GetStrongTypeInfo() is { } targetInfo)
        {
            var fromPrimitive = GetConverter(targetInfo, nameof(Converters<,>.FromPrimitive));

            if (source == targetInfo.PrimitiveType)
            {
                converter = fromPrimitive;
                return true;
            }

            if (root(source, targetInfo.PrimitiveType, out var convertSource))
            {
                converter = value => fromPrimitive(convertSource(value));
                return true;
            }
        }

        converter = null;
        return false;
    }

    private static ChangeType GetConverter(StrongTypeInfo info, string name)
    {
        return (ChangeType)typeof(Converters<,>)
            .MakeGenericType(info.ClrType, info.PrimitiveType)
            .GetField(name)!
            .GetValue(null)!;
    }

    private static class Converters<TStrongType, TPrimitive>
        where TStrongType : IStrongType<TStrongType, TPrimitive>
        where TPrimitive : notnull
    {
        public static readonly ChangeType FromPrimitive = static value => TStrongType.From((TPrimitive)value!);
        public static readonly ChangeType ToPrimitive = static value => ((TStrongType)value!).AsPrimitive();
    }
}
