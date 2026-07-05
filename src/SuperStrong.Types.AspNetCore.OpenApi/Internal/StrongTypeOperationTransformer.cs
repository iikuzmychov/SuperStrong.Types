using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        foreach (var parameterDescription in context.Description.ParameterDescriptions)
        {
            if (GetStrongTypeInfo(parameterDescription) is not { } strongTypeInfo)
            {
                continue;
            }

            if (parameterDescription.Source == BindingSource.Body)
            {
                await TransformRequestBodyAsync(operation, context, strongTypeInfo, cancellationToken);
            }
            else
            {
                await TransformParameterAsync(operation, context, parameterDescription, strongTypeInfo, cancellationToken);
            }
        }
    }

    private static StrongTypeInfo? GetStrongTypeInfo(ApiParameterDescription parameterDescription)
    {
        if (parameterDescription.Type.GetStrongTypeInfo() is { } strongTypeInfo)
        {
            return strongTypeInfo;
        }

        // MVC's ApiExplorer describes TypeConverter-bindable parameters as plain strings,
        // so the strong type has to be recovered from the declared parameter type
        return parameterDescription.ParameterDescriptor.ParameterType.GetStrongTypeInfo();
    }

    private async Task TransformParameterAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        ApiParameterDescription parameterDescription,
        StrongTypeInfo strongTypeInfo,
        CancellationToken cancellationToken)
    {
        var parameter = operation
            .Parameters?
            .OfType<OpenApiParameter>()
            .FirstOrDefault(parameter => parameter.Name == parameterDescription.Name);

        if (parameter is null)
        {
            return;
        }

        parameter.Schema = await CreateSchemaAsync(context, strongTypeInfo, parameterDescription, cancellationToken);
    }

    private async Task TransformRequestBodyAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        StrongTypeInfo strongTypeInfo,
        CancellationToken cancellationToken)
    {
        if (operation.RequestBody?.Content is not { } content)
        {
            return;
        }

        var schema = await CreateSchemaAsync(context, strongTypeInfo, parameterDescription: null, cancellationToken);

        foreach (var mediaType in content.Values)
        {
            mediaType.Schema = schema;
        }
    }

    private async Task<IOpenApiSchema> CreateSchemaAsync(
        OpenApiOperationTransformerContext context,
        StrongTypeInfo strongTypeInfo,
        ApiParameterDescription? parameterDescription,
        CancellationToken cancellationToken)
    {
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
                return schema;

            case StrongTypeOpenApiRepresentation.Reference:
                var componentName = strongTypeInfo.ClrType.Name;

                context.Document!.AddComponent(componentName, schema);

                return new OpenApiSchemaReference(componentName, context.Document);

            default:
                throw new UnreachableException($"Unexpected {nameof(StrongTypeOpenApiRepresentation)} value.");
        }
    }
}
