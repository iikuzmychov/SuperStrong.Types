using HotChocolate.Configuration;
using HotChocolate.Internal;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Configurations;
using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeScalarInterceptor(StrongTypeHotChocolateOptions options) : TypeInterceptor
{
    public override void OnBeforeRegisterDependencies(
        ITypeDiscoveryContext discoveryContext,
        TypeSystemConfiguration configuration)
    {
        if (configuration is not ScalarTypeConfiguration scalarConfiguration)
        {
            return;
        }

        if (discoveryContext.Type is not ScalarType scalar)
        {
            return;
        }

        var runtimeType = scalar.RuntimeType;

        if (runtimeType.GetStrongTypeInfo() is not { } info)
        {
            return;
        }

        var inspector = discoveryContext.TypeInspector;
        var groups = info.Definition.Validators.GroupBy(validator => validator.GetType());

        foreach (var group in groups)
        {
            if (options.ResolveAdapter(group.Key) is not { } build)
            {
                continue;
            }

            foreach (var directive in build(group.ToList()))
            {
                scalarConfiguration.AddDirective(directive, inspector);
            }
        }
    }
}
