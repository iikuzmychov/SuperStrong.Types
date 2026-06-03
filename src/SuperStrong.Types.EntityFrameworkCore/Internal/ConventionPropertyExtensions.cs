using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal static class ConventionPropertyExtensions
{
    public const string ResolvedRelationalTypeMappingAnnotation = "SuperStrong:ResolvedRelationalTypeMapping";

    public static RelationalTypeMapping? FindResolvedRelationalTypeMapping(this IConventionProperty property)
    {
        ArgumentNullException.ThrowIfNull(property);

        return property.FindAnnotation(ResolvedRelationalTypeMappingAnnotation)?.Value as RelationalTypeMapping;
    }
}
