using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types;

public sealed record StrongTypeValidationResult
{
    private static readonly StrongTypeValidationResult ValidResult = new(errorMessage: null);

    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsValid { get; }

    public string? ErrorMessage { get; }

    private StrongTypeValidationResult(string? errorMessage)
    {
        IsValid = errorMessage is null;
        ErrorMessage = errorMessage;
    }

    public static StrongTypeValidationResult Valid() => ValidResult;

    public static StrongTypeValidationResult Invalid(string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(errorMessage);

        return new StrongTypeValidationResult(errorMessage);
    }
}
