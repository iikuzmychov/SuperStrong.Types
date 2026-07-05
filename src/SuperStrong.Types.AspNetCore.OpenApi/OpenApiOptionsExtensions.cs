using Microsoft.AspNetCore.OpenApi;
using SuperStrong.Types.AspNetCore.OpenApi.Internal;
using SuperStrong.Types.Reflection;
using System.Diagnostics;

namespace SuperStrong.Types.AspNetCore.OpenApi;

public static class OpenApiOptionsExtensions
{
    public static OpenApiOptions AddStrongTypes(
        this OpenApiOptions options,
        StrongTypeOpenApiRepresentation representation)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (!Enum.IsDefined(representation))
        {
            throw new ArgumentException(
                $"The value '{representation}' is not a valid {typeof(StrongTypeOpenApiRepresentation).Name}.",
                paramName: nameof(representation));
        }

        options.AddSchemaTransformer(new StrongTypeSchemaTransformer(representation));
        options.AddOperationTransformer(new StrongTypeOperationTransformer(representation));
        options.ConfigureCreateSchemaReferenceId(representation);

        return options;
    }

    private static void ConfigureCreateSchemaReferenceId(
        this OpenApiOptions options,
        StrongTypeOpenApiRepresentation representation)
    {
        switch (representation)
        {
            case StrongTypeOpenApiRepresentation.Inline:
                var createPrimitiveSchemaReferenceId = options.CreateSchemaReferenceId;

                options.CreateSchemaReferenceId = jsonTypeInfo =>
                {
                    return jsonTypeInfo.Type.GetStrongTypeInfo() is not null
                        ? null
                        : createPrimitiveSchemaReferenceId(jsonTypeInfo);
                };

                break;

            case StrongTypeOpenApiRepresentation.Reference:
                return;

            default:
                throw new UnreachableException($"Unexpected {nameof(StrongTypeOpenApiRepresentation)} value.");
        }
    }
}
