using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using SuperStrong.Types.Reflection;
using System.Diagnostics;
using System.Text.Json.Serialization.Metadata;

namespace SuperStrong.Types.AspNetCore.OpenApi.Internal;

internal sealed class StrongTypeSchemaTransformer : IOpenApiSchemaTransformer
{
    private readonly StrongTypeOpenApiRepresentation _representation;

    public StrongTypeSchemaTransformer(StrongTypeOpenApiRepresentation representation)
    {
        _representation = representation;
    }

    public async Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var jsonTypeInfo = context.JsonTypeInfo;

        if (jsonTypeInfo.Type.GetStrongTypeInfo() is { } strongTypeInfo)
        {
            var primitiveSchema = await context.GetOrCreateSchemaAsync(
                strongTypeInfo.PrimitiveType,
                parameterDescription: null,
                cancellationToken);

            schema.Type = primitiveSchema.Type;
            schema.Format = primitiveSchema.Format;
            schema.Properties?.Clear();

            return;
        }

        if (jsonTypeInfo.Kind is JsonTypeInfoKind.Enumerable &&
            jsonTypeInfo.ElementType!.GetStrongTypeInfo() is { } itemStrongTypeInfo)
        {
            schema.Items = await CreateElementSchemaAsync(
                context, itemStrongTypeInfo, jsonTypeInfo.ElementType!, cancellationToken);
        }

        if (jsonTypeInfo.Kind is JsonTypeInfoKind.Dictionary)
        {
            if (jsonTypeInfo.ElementType!.GetStrongTypeInfo() is { } valueStrongTypeInfo)
            {
                schema.AdditionalProperties = await CreateElementSchemaAsync(
                    context, valueStrongTypeInfo, jsonTypeInfo.ElementType!, cancellationToken);
            }

            if (jsonTypeInfo.KeyType!.GetStrongTypeInfo() is { } keyStrongTypeInfo)
            {
                schema.PropertyNames = await CreateKeySchemaAsync(
                    context, keyStrongTypeInfo, jsonTypeInfo.KeyType!, cancellationToken);
            }
        }
    }

    private async Task<OpenApiSchema> CreateElementSchemaAsync(
        OpenApiSchemaTransformerContext context,
        StrongTypeInfo elementStrongTypeInfo,
        Type elementType,
        CancellationToken cancellationToken)
    {
        var primitiveSchema = await context.GetOrCreateSchemaAsync(
            elementStrongTypeInfo.PrimitiveType,
            parameterDescription: null,
            cancellationToken);

        var elementSchema = new OpenApiSchema
        {
            Type = primitiveSchema.Type,
            Format = primitiveSchema.Format,
        };

        switch (_representation)
        {
            case StrongTypeOpenApiRepresentation.Inline:
                break;

            case StrongTypeOpenApiRepresentation.Reference:
                elementSchema.Metadata = new Dictionary<string, object>
                {
                    // makes the resolution pass replace this element with a `$ref` to a component named after the strong type
                    ["x-schema-id"] = elementType.Name,
                };
                break;

            default:
                Debug.Fail($"Unexpected {nameof(StrongTypeOpenApiRepresentation)} value.");
                break;
        }

        return elementSchema;
    }

    private async Task<IOpenApiSchema> CreateKeySchemaAsync(
        OpenApiSchemaTransformerContext context,
        StrongTypeInfo keyStrongTypeInfo,
        Type keyType,
        CancellationToken cancellationToken)
    {
        var primitiveSchema = await context.GetOrCreateSchemaAsync(
            keyStrongTypeInfo.PrimitiveType,
            parameterDescription: null,
            cancellationToken);

        var keySchema = new OpenApiSchema
        {
            Type = primitiveSchema.Type,
            Format = primitiveSchema.Format,
        };

        switch (_representation)
        {
            case StrongTypeOpenApiRepresentation.Inline:
                return keySchema;

            case StrongTypeOpenApiRepresentation.Reference:
                context.Document!.AddComponent(keyType.Name, keySchema);
                return new OpenApiSchemaReference(keyType.Name, context.Document);

            default:
                Debug.Fail($"Unexpected {nameof(StrongTypeOpenApiRepresentation)} value.");
                return default;
        }
    }
}
