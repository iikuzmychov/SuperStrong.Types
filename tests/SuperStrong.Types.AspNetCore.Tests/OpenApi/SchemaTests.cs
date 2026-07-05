using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using SuperStrong.Types.AspNetCore.OpenApi;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.AspNetCore.Tests.OpenApi;

public abstract class SchemaTests<TStrongType, TPrimitive>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    public sealed record Dto(TStrongType Strong, TPrimitive Primitive);

    [Fact]
    public async Task Maps_a_body_property_to_the_primitive_schema_when_inline()
    {
        var document = await GetDocumentAsync(
            StrongTypeOpenApiRepresentation.Inline,
            app => app.MapPost("/", (Dto body) => body));

        var properties = GetRequestBodySchema(document).Properties!;
        var strongTypeSchema = Assert.IsType<OpenApiSchema>(properties["strong"]);

        AssertPrimitiveSchema(properties["primitive"], strongTypeSchema);
    }

    [Fact]
    public async Task Maps_a_body_property_to_a_primitive_schema_reference()
    {
        var document = await GetDocumentAsync(
            StrongTypeOpenApiRepresentation.Reference,
            app => app.MapPost("/", (Dto body) => body));

        var properties = GetRequestBodySchema(document).Properties!;
        var strongTypeSchema = AssertStrongTypeReference(properties["strong"]);

        AssertPrimitiveSchema(properties["primitive"], strongTypeSchema);
    }

    [Fact]
    public async Task Maps_route_and_query_parameters_to_the_primitive_schema_when_inline()
    {
        var document = await GetDocumentAsync(
            StrongTypeOpenApiRepresentation.Inline,
            app => app.MapGet("/{route}", (TStrongType route, TStrongType query, TPrimitive primitive) => Results.Ok()));

        var primitiveSchema = GetParameterSchema(document, "primitive");
        var routeSchema = Assert.IsType<OpenApiSchema>(GetParameterSchema(document, "route"));
        var querySchema = Assert.IsType<OpenApiSchema>(GetParameterSchema(document, "query"));

        AssertPrimitiveSchema(primitiveSchema, routeSchema);
        AssertPrimitiveSchema(primitiveSchema, querySchema);
    }

    [Fact]
    public async Task Maps_route_and_query_parameters_to_primitive_schema_references()
    {
        var document = await GetDocumentAsync(
            StrongTypeOpenApiRepresentation.Reference,
            app => app.MapGet("/{route}", (TStrongType route, TStrongType query, TPrimitive primitive) => Results.Ok()));

        var primitiveSchema = GetParameterSchema(document, "primitive");
        var routeSchema = AssertStrongTypeReference(GetParameterSchema(document, "route"));
        var querySchema = AssertStrongTypeReference(GetParameterSchema(document, "query"));

        AssertPrimitiveSchema(primitiveSchema, routeSchema);
        AssertPrimitiveSchema(primitiveSchema, querySchema);
    }

    private static async Task<OpenApiDocument> GetDocumentAsync(
        StrongTypeOpenApiRepresentation representation,
        Action<WebApplication> configure)
    {
        await using var application = await TestApplication.StartAsync(
            builder => builder.Services.AddOpenApi(options => options.AddStrongTypes(representation)),
            app =>
            {
                app.MapOpenApi();
                configure(app);
            });

        var openApiJson = await application.Client.GetStringAsync(
            "/openapi/v1.json", TestContext.Current.CancellationToken);

        return OpenApiDocument.Parse(openApiJson).Document!;
    }

    private static IOpenApiSchema GetRequestBodySchema(OpenApiDocument document)
    {
        return document.Paths["/"].Operations![HttpMethod.Post].RequestBody!.Content!["application/json"].Schema!;
    }

    private static IOpenApiSchema GetParameterSchema(OpenApiDocument document, string name)
    {
        var parameters = document.Paths["/{route}"].Operations![HttpMethod.Get].Parameters!;

        return parameters.Single(parameter => parameter.Name == name).Schema!;
    }

    private static IOpenApiSchema AssertStrongTypeReference(IOpenApiSchema schema)
    {
        var reference = Assert.IsType<OpenApiSchemaReference>(schema);

        Assert.Equal(typeof(TStrongType).Name, reference.Reference.Id);

        return reference;
    }

    private static void AssertPrimitiveSchema(IOpenApiSchema primitiveSchema, IOpenApiSchema strongTypeSchema)
    {
        Assert.Equal(primitiveSchema.Type, strongTypeSchema.Type);
        Assert.Equal(primitiveSchema.Format, strongTypeSchema.Format);
    }
}

public sealed class BoolSchemaTests : SchemaTests<StrongBool, bool>;

public sealed class ByteSchemaTests : SchemaTests<StrongByte, byte>;

public sealed class SByteSchemaTests : SchemaTests<StrongSByte, sbyte>;

public sealed class ShortSchemaTests : SchemaTests<StrongShort, short>;

public sealed class UShortSchemaTests : SchemaTests<StrongUShort, ushort>;

public sealed class IntSchemaTests : SchemaTests<StrongInt, int>;

public sealed class UIntSchemaTests : SchemaTests<StrongUInt, uint>;

public sealed class LongSchemaTests : SchemaTests<StrongLong, long>;

public sealed class ULongSchemaTests : SchemaTests<StrongULong, ulong>;

public sealed class FloatSchemaTests : SchemaTests<StrongFloat, float>;

public sealed class DoubleSchemaTests : SchemaTests<StrongDouble, double>;

public sealed class DecimalSchemaTests : SchemaTests<StrongDecimal, decimal>;

public sealed class StringSchemaTests : SchemaTests<StrongString, string>;

public sealed class CharSchemaTests : SchemaTests<StrongChar, char>;

public sealed class GuidSchemaTests : SchemaTests<StrongGuid, Guid>;

public sealed class DateTimeSchemaTests : SchemaTests<StrongDateTime, DateTime>;

public sealed class DateTimeOffsetSchemaTests : SchemaTests<StrongDateTimeOffset, DateTimeOffset>;

public sealed class DateOnlySchemaTests : SchemaTests<StrongDateOnly, DateOnly>;

public sealed class TimeOnlySchemaTests : SchemaTests<StrongTimeOnly, TimeOnly>;

public sealed class TimeSpanSchemaTests : SchemaTests<StrongTimeSpan, TimeSpan>;
