using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace SuperStrong.Types;

public sealed class StrongTypeValidationException : Exception
{
    public Type StrongType { get; }
    public object Value { get; }
    public ImmutableArray<string> ErrorMessages { get; }

    internal StrongTypeValidationException(Type strongType, object value, ImmutableArray<string> errorMessages)
        : base(CreateMessage(strongType, errorMessages))
    {
        Debug.Assert(value is not null);
        Debug.Assert(strongType is not null);
        Debug.Assert(!errorMessages.IsDefaultOrEmpty);

        StrongType = strongType;
        Value = value;
        ErrorMessages = errorMessages;
    }

    private static string CreateMessage(Type strongType, ImmutableArray<string> errorMessages)
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.Append($"Validation failed for strong type '{strongType}'. Errors:");

        foreach (var errorMessage in errorMessages)
        {
            messageBuilder.AppendLine();
            messageBuilder.Append("- ");
            messageBuilder.Append(errorMessage);
        }

        return messageBuilder.ToString();
    }
}
