using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        DecorateValueConverterSelector(services);
    }

    private static void DecorateValueConverterSelector(IServiceCollection services)
    {
        var targetDescriptor = services.FirstOrDefault(
            descriptor => descriptor.ServiceType == typeof(IValueConverterSelector));

        if (targetDescriptor is null)
        {
            throw new InvalidOperationException(
                $"No {nameof(IValueConverterSelector)} is configured yet. " +
                $"Make sure to configure a database provider first.");
        }

        services.Replace(
            ServiceDescriptor.Describe(
                typeof(IValueConverterSelector),
                serviceProvider =>
                {
                    var inner = (IValueConverterSelector)CreateTargetInstance(serviceProvider, targetDescriptor);
                    return new StrongTypeValueConverterSelector(inner);
                },
                targetDescriptor.Lifetime));
    }

    private static object CreateTargetInstance(IServiceProvider serviceProvider, ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance is not null)
        {
            return descriptor.ImplementationInstance;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            return descriptor.ImplementationFactory(serviceProvider);
        }

        if (descriptor.ImplementationType is not null)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, descriptor.ImplementationType);
        }

        throw new InvalidOperationException("Cannot create target instance for the service descriptor.");
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
