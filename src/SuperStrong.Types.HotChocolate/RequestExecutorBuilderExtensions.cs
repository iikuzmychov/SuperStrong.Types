using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.HotChocolate.Internal;

namespace SuperStrong.Types.HotChocolate;

public static class RequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddStrongTypes(
        this IRequestExecutorBuilder builder,
        Action<StrongTypeHotChocolateOptionsBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var options = new StrongTypeHotChocolateOptions();
        var optionsBuilder = new StrongTypeHotChocolateOptionsBuilder(options);

        optionsBuilder.AddDefaults();
        configure?.Invoke(optionsBuilder);

        foreach (var directiveType in options.DirectiveTypes)
        {
            builder.AddDirectiveType(directiveType);
        }

        builder.TryAddTypeInterceptor(new StrongTypeScalarInterceptor(options));

        return builder;
    }
}
