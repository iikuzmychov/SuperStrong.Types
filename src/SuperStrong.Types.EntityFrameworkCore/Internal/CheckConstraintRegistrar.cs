using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal static class CheckConstraintRegistrar
{
    public static void TryRegister<TPrimitive>(
        IConventionProperty property,
        string purpose,
        Func<string, RelationalTypeMapping, string> buildSql)
        where TPrimitive : notnull
    {
        var entityType = ResolveEntityType(property);

        if (StoreObjectIdentifier.Create(entityType, StoreObjectType.Table) is not { } storeObject)
        {
            return;
        }

        var columnName = property.GetColumnName(storeObject);

        if (columnName is null)
        {
            return;
        }

        if (property.FindResolvedRelationalTypeMapping() is not { } mapping)
        {
            return;
        }

        if ((mapping.Converter?.ProviderClrType ?? mapping.ClrType) != typeof(TPrimitive))
        {
            return;
        }

        var sql = buildSql(columnName, mapping);

        entityType.Builder.HasCheckConstraint(
            name: $"CK_{entityType.ShortName()}_{columnName}_{purpose}",
            sql: sql);
    }

    private static IConventionEntityType ResolveEntityType(IConventionProperty property)
    {
        return property.DeclaringType switch
        {
            IConventionEntityType entity => entity,
            IConventionComplexType => property.DeclaringType.ContainingEntityType,

            _ => throw new InvalidOperationException(
                $"Declaring type '{property.DeclaringType.DisplayName()}' of property '{property.Name}' " +
                $"is neither an entity nor a complex type."),
        };
    }
}
