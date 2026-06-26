namespace SuperStrong.Types.Tests;

public sealed class CharPrimitiveData : TheoryData<char>
{
    public CharPrimitiveData()
    {
        Add('a');
        Add('Z');
        Add(' ');
    }
}
