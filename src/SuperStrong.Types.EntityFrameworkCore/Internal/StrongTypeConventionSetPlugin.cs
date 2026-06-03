using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeConventionSetPlugin(
    StrongTypeDbContextOptionsExtension extension,
    IServiceProvider serviceProvider)
    : IConventionSetPlugin
{
    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        var typeMappingSource = serviceProvider.GetRequiredService<IRelationalTypeMappingSource>();

        conventionSet.ModelFinalizingConventions.Add(
            new StrongTypeConvention(extension.Adapters, typeMappingSource));

        return conventionSet;
    }
}
