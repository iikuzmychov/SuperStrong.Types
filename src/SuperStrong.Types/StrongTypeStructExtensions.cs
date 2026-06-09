namespace SuperStrong.Types;

public static class StrongTypeStructExtensions
{
    extension<TStrongType, TPrimitive>(IStrongType<TStrongType, TPrimitive>)
        where TStrongType : struct, IStrongType<TStrongType, TPrimitive>
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

        public static bool TryFrom(TPrimitive? value, out TStrongType? result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            if (TStrongType.TryFrom(value.Value, out var strongType))
            {
                result = strongType;
                return true;
            }

            result = null;
            return false;
        }
    }

    extension<TStrongType, TPrimitive>(IStrongType<TStrongType, TPrimitive>)
        where TStrongType : struct, IStrongType<TStrongType, TPrimitive>
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

        public static bool TryFrom(TPrimitive? value, out TStrongType? result)
        {
            if (value is null)
            {
                result = null;
                return true;
            }

            if (TStrongType.TryFrom(value, out var strongType))
            {
                result = strongType;
                return true;
            }

            result = null;
            return false;
        }
    }
}
