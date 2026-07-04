namespace SuperStrong.Types;

public static class StrongTypeValidationResultExtensions
{
    extension(StrongTypeValidationResult)
    {
        public static StrongTypeValidationResult Valid() => StrongTypeValidationResult.Valid.Instance;

        public static StrongTypeValidationResult Invalid(string errorMessage)
        {
            ArgumentNullException.ThrowIfNull(errorMessage);

            return new StrongTypeValidationResult.Invalid(errorMessage);
        }
    }
}
