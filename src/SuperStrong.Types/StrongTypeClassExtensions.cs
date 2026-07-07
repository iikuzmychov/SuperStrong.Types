namespace SuperStrong.Types;

public static class StrongTypeClassExtensions
{
    extension<TStrongType, TPrimitive>(IStrongType<TStrongType, TPrimitive>)
        where TStrongType : class, IStrongType<TStrongType, TPrimitive>
        where TPrimitive : struct
    {
        public static TStrongType? FromNullable(TPrimitive? value)
        {
            if (value is null)
            {
                return null;
            }

            return TStrongType.From(value.Value);
        }

        public static bool TryFromNullable(TPrimitive? value, out TStrongType? result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            return TStrongType.TryFrom(value.Value, out result);
        }
    }

    extension<TStrongType, TPrimitive>(IStrongType<TStrongType, TPrimitive>)
        where TStrongType : class, IStrongType<TStrongType, TPrimitive>
        where TPrimitive : class
    {
        public static TStrongType? FromNullable(TPrimitive? value)
        {
            if (value is null)
            {
                return null;
            }

            return TStrongType.From(value);
        }

        public static bool TryFromNullable(TPrimitive? value, out TStrongType? result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            return TStrongType.TryFrom(value, out result);
        }
    }
}
