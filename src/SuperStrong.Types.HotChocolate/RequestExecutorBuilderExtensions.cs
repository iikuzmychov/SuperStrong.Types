using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.HotChocolate.Internal;

namespace SuperStrong.Types.HotChocolate;

public static class RequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddStrongTypes(this IRequestExecutorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddDirectiveType(typeof(StrongTypeDirectiveType));
        builder.AddTypeDiscoveryHandler(context => new StrongTypeDiscoveryHandler(context.TypeInspector));

        return builder;
    }
}
