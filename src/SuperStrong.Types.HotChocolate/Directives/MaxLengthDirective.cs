namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record MaxLengthDirective
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
