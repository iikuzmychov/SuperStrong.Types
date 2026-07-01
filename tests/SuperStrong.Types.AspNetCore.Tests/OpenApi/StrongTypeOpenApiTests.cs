using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.AspNetCore.OpenApi;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.AspNetCore.Tests.OpenApi;

public sealed partial class StrongTypeOpenApiTests
{
    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_each_primitive_family(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapPost("/", (PrimitivesDto body) => body));
    }

    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_nullable_property(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapPost("/", (NullableDto body) => body));
    }

    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_collection(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapPost("/", (CollectionDto body) => body));
    }

    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_dictionary(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapPost("/", (DictionaryDto body) => body));
    }

    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_nested_containers(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapPost("/", (NestedDto body) => body));
    }

    [Theory]
    [InlineData(StrongTypeOpenApiRepresentation.Inline)]
    [InlineData(StrongTypeOpenApiRepresentation.Reference)]
    public Task Maps_route_and_query_parameters(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(representation, app => app.MapGet("/{id}", (UserId id, Age? age) => Results.Ok()));
    }

    private static async Task VerifyDocumentAsync(
        StrongTypeOpenApiRepresentation representation,
        Action<WebApplication> configure)
    {
        var builder = WebApplication.CreateSlimBuilder();
        
        builder.WebHost.UseTestServer();
        builder.Services.AddOpenApi(options => options.AddStrongTypes(representation));

        await using var app = builder.Build();
        
        app.MapOpenApi();
        configure(app);

        await app.StartAsync(TestContext.Current.CancellationToken);

        using var client = app.GetTestClient();
        var openApiJson = await client.GetStringAsync("/openapi/v1.json", TestContext.Current.CancellationToken);

        await VerifyJson(openApiJson).UseParameters(representation);
    }

    public sealed record PrimitivesDto(
        StrongBool Bool,
        StrongByte Byte,
        StrongSByte SByte,
        StrongShort Short,
        StrongUShort UShort,
        StrongInt Int,
        StrongUInt UInt,
        StrongLong Long,
        StrongULong ULong,
        StrongFloat Float,
        StrongDouble Double,
        StrongDecimal Decimal,
        StrongString String,
        StrongChar Char,
        StrongGuid Guid,
        StrongDateTime DateTime,
        StrongDateTimeOffset DateTimeOffset,
        StrongDateOnly DateOnly,
        StrongTimeOnly TimeOnly,
        StrongTimeSpan TimeSpan);

    [StrongType<Guid>] public sealed partial class UserId;
    [StrongType<int>] public sealed partial class Age;
    [StrongType<string>] public sealed partial class Tag;

    public sealed record NullableDto(UserId? Id);

    public sealed record CollectionDto(IReadOnlyList<Tag> Tags);

    public sealed record DictionaryDto(
        IReadOnlyDictionary<string, Tag> TagsByName,
        IReadOnlyDictionary<UserId, Tag> TagsByUserId);

    public sealed record NestedDto(
        IReadOnlyList<IReadOnlyList<Tag>> TagGroups,
        IReadOnlyDictionary<string, IReadOnlyList<Tag>> TagsByGroup,
        IReadOnlyList<IReadOnlyDictionary<string, Tag>> TagMaps);
}
