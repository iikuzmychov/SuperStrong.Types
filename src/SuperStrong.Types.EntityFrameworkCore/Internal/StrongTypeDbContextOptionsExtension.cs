using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeDbContextOptionsExtension : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info { get; }

    public StrongTypeDbContextOptionsExtension()
    {
        Info = new ExtensionInfo(this);
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.TryAddSingleton(
            provider => new Lazy<IRelationalTypeMappingSource>(provider.GetRequiredService<IRelationalTypeMappingSource>));

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IRelationalTypeMappingSourcePlugin, StrongTypeRelationalTypeMappingSourcePlugin>());
    }

    private sealed class ExtensionInfo(StrongTypeDbContextOptionsExtension extension)
        : DbContextOptionsExtensionInfo(extension)
    {
        public override bool IsDatabaseProvider => false;
        public override string LogFragment => string.Empty;

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ExtensionInfo;
        }
    }
}
