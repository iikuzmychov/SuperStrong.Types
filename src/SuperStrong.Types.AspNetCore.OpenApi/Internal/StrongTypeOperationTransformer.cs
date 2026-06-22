using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using SuperStrong.Types.Reflection;
using System.Diagnostics;

namespace SuperStrong.Types.AspNetCore.OpenApi.Internal;

internal sealed class StrongTypeOperationTransformer : IOpenApiOperationTransformer
{
    private readonly StrongTypeOpenApiRepresentation _representation;

    public StrongTypeOperationTransformer(StrongTypeOpenApiRepresentation representation)
    {
        _representation = representation;
    }

    public async Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (operation.Parameters is null)
        {
            return;
        }

        foreach (var parameterDescription in context.Description.ParameterDescriptions)
        {
            if (parameterDescription.Type?.GetStrongTypeInfo() is not { } strongTypeInfo)
            {
                continue;
            }

            var parameter = operation
                .Parameters
                .OfType<OpenApiParameter>()
                .FirstOrDefault(parameter => parameter.Name == parameterDescription.Name);

            if (parameter is null)
            {
                continue;
            }

            var primitiveSchema = await context.GetOrCreateSchemaAsync(
                strongTypeInfo.PrimitiveType,
                parameterDescription,
                cancellationToken);

            var schema = new OpenApiSchema
            {
                Type = primitiveSchema.Type,
                Format = primitiveSchema.Format,
            };

            switch (_representation)
            {
                case StrongTypeOpenApiRepresentation.Inline:
                    parameter.Schema = schema;
                    break;

                case StrongTypeOpenApiRepresentation.Reference:
                    context.Document!.AddComponent(parameterDescription.Type.Name, schema);
                    parameter.Schema = new OpenApiSchemaReference(parameterDescription.Type.Name, context.Document);
                    break;

                default:
                    Debug.Fail($"Unexpected {nameof(StrongTypeOpenApiRepresentation)} value.");
                    break;
            }
        }
    }
}
