using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed record LocationInfo(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan)
{
    public Location ToLocation() => Location.Create(FilePath, TextSpan, LineSpan);

    public static LocationInfo? From(SyntaxNode node)
    {
        var location = node.GetLocation();
        var sourceTree = location.SourceTree;

        if (sourceTree is null)
        {
            return null;
        }

        return new LocationInfo(sourceTree.FilePath, location.SourceSpan, location.GetLineSpan().Span);
    }
}
