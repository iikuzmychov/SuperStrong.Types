using System.Text;

namespace SuperStrong.Types.Generators.Helpers;

internal sealed class IndentedWriter(StringBuilder builder)
{
    private int _indent;

    public void Line()
    {
        builder.AppendLine();
    }

    public void Line(string text)
    {
        for (var i = 0; i < _indent; i++)
        {
            builder.Append("    ");
        }
        builder.AppendLine(text);
    }

    public IDisposable Block(string declaration)
    {
        Line(declaration);
        Line("{");
        _indent++;

        return new BlockScope(this);
    }

    public override string ToString() => builder.ToString();

    private sealed class BlockScope(IndentedWriter writer) : IDisposable
    {
        public void Dispose()
        {
            writer._indent--;
            writer.Line("}");
        }
    }
}
