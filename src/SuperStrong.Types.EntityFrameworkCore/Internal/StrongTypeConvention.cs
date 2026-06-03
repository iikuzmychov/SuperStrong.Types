using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.Reflection;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeConvention(
    IReadOnlyList<StrongTypeValidatorAdapter> validatorAdapters,
    IRelationalTypeMappingSource typeMappingSource)
    : IModelFinalizingConvention
{
    private readonly ConcurrentDictionary<Type, StrongTypeValidatorAdapter?> _resolvedValidatorAdapters = new();

    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        if (validatorAdapters.Count == 0)
        {
            return;
        }

        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var property in entityType.GetDeclaredProperties())
            {
                if (property.ClrType.GetStrongTypeInfo() is not { } strongTypeInfo)
                {
                    continue;
                }

                AnnotateRelationalTypeMapping(property);

                foreach (var group in strongTypeInfo.Definition.Validators.GroupBy(validator => validator.GetType()))
                {
                    var validatorAdapter = _resolvedValidatorAdapters.GetOrAdd(group.Key, ResolveValidatorAdapter);

                    validatorAdapter?.Apply(
                        group.Key,
                        group.ToImmutableArray(),
                        property,
                        strongTypeInfo.StrongType);
                }
            }
        }
    }

    private void AnnotateRelationalTypeMapping(IConventionProperty property)
    {
        if (property.FindAnnotation(ConventionPropertyExtensions.ResolvedRelationalTypeMappingAnnotation) is not null)
        {
            return;
        }

        var mapping = typeMappingSource.FindMapping((IProperty)property);

        if (mapping is null)
        {
            return;
        }

        property.SetAnnotation(ConventionPropertyExtensions.ResolvedRelationalTypeMappingAnnotation, mapping);
    }

    private StrongTypeValidatorAdapter? ResolveValidatorAdapter(Type validatorType)
    {
        foreach (var adapter in validatorAdapters)
        {
            if (adapter.CanHandle(validatorType))
            {
                return adapter;
            }
        }

        return null;
    }
}
