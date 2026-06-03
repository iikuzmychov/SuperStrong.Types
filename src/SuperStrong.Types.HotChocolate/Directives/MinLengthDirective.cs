namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record MinLengthDirective
{
    public required int Value
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(Value));

            field = value;
        }
    }
}
