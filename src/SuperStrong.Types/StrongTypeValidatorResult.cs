using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types;

public sealed record StrongTypeValidatorResult
{
    private static readonly StrongTypeValidatorResult ValidResult = new(errorMessage: null);

    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsValid { get; }

    public string? ErrorMessage { get; }

    private StrongTypeValidatorResult(string? errorMessage)
    {
        IsValid = errorMessage is null;
        ErrorMessage = errorMessage;
    }

    public static StrongTypeValidatorResult Valid() => ValidResult;

    public static StrongTypeValidatorResult Invalid(string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(errorMessage);

        return new StrongTypeValidatorResult(errorMessage);
    }
}
