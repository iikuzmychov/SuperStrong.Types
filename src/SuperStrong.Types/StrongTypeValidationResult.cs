using System.Diagnostics;

namespace SuperStrong.Types;

public abstract record StrongTypeValidationResult
{
    private StrongTypeValidationResult()
    {
    }

    public sealed record Valid : StrongTypeValidationResult
    {
        internal static readonly Valid Instance = new();

        private Valid()
        {
        }
    }

    public sealed record Invalid : StrongTypeValidationResult
    {
        public string ErrorMessage { get; }

        internal Invalid(string errorMessage)
        {
            Debug.Assert(errorMessage is not null);

            ErrorMessage = errorMessage;
        }
    }
}
