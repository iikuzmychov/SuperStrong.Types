namespace SuperStrong.Types.Tests;

public sealed class StringPrimitiveData : TheoryData<string>
{
    public StringPrimitiveData()
    {
        Add("hello");
        Add("");
        Add(" ");
        Add("a\"b\\c");
        Add("café");
    }
}
