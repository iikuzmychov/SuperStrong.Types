using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.HotChocolate.Internal;
using System.Diagnostics;

namespace SuperStrong.Types.HotChocolate;

public static class RequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddStrongTypes(
        this IRequestExecutorBuilder builder,
        StrongTypeGraphQLRepresentation representation)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!Enum.IsDefined(representation))
        {
            throw new ArgumentException(
                $"The value '{representation}' is not a valid {typeof(StrongTypeGraphQLRepresentation).Name}.",
                paramName: nameof(representation));
        }

        switch (representation)
        {
            case StrongTypeGraphQLRepresentation.PrimitiveType:
                builder.TryAddTypeInterceptor<PrimitiveTypeRepresentationInterceptor>();
                builder.AddTypeConverter<StrongTypeChangeTypeProvider>();
                break;

            case StrongTypeGraphQLRepresentation.StrongType:
                builder.AddDirectiveType(typeof(StrongTypeDirectiveType));
                builder.AddTypeDiscoveryHandler(context => new StrongTypeDiscoveryHandler(context.TypeInspector));
                break;

            default:
                throw new UnreachableException($"Unexpected {nameof(StrongTypeGraphQLRepresentation)} value.");
        }

        return builder;
    }
}
