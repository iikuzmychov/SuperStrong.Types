using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.AspNetCore.OpenApi;

namespace SuperStrong.Types.AspNetCore.Tests.OpenApi;

public sealed partial class DocumentTests
{
    public static TheoryData<StrongTypeOpenApiRepresentation> Representations { get; } =
    [
        StrongTypeOpenApiRepresentation.PrimitiveType,
        StrongTypeOpenApiRepresentation.StrongType,
    ];

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_nullable_property(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(
            representation,
            app =>
            {
                app.MapPost("/", (NullableDto body) => body);
            });
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_collection(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(
            representation,
            app =>
            {
                app.MapPost("/", (CollectionDto body) => body);
            });
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_dictionary(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(
            representation,
            app =>
            {
                app.MapPost("/", (DictionaryDto body) => body);
            });
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_nested_containers(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(
            representation,
            app =>
            {
                app.MapPost("/", (NestedDto body) => body);
            });
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_route_and_query_parameters(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyDocumentAsync(
            representation,
            app =>
            {
                app.MapGet("/{id}", (UserId id, Age? age) => Results.Ok());
            });
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_controller_route_parameter(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyControllerDocumentAsync<RouteController<UserId>>(representation);
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_controller_query_parameter(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyControllerDocumentAsync<QueryController<UserId>>(representation);
    }

    [Theory]
    [MemberData(nameof(Representations))]
    public Task Maps_controller_body(StrongTypeOpenApiRepresentation representation)
    {
        return VerifyControllerDocumentAsync<BodyController<UserId>>(representation);
    }

    private static async Task VerifyControllerDocumentAsync<TController>(StrongTypeOpenApiRepresentation representation)
        where TController : ControllerBase
    {
        await using var application = await TestApplication.StartAsync<TController>(
            builder =>
            {
                builder.Services.AddOpenApi(options => options.AddStrongTypes(representation));
            },
            app =>
            {
                app.MapOpenApi();
            });

        await VerifyDocumentAsync(application, representation);
    }

    private static async Task VerifyDocumentAsync(
        StrongTypeOpenApiRepresentation representation,
        Action<WebApplication> configure)
    {
        await using var application = await TestApplication.StartAsync(
            builder =>
            {
                builder.Services.AddOpenApi(options => options.AddStrongTypes(representation));
            },
            app =>
            {
                app.MapOpenApi();
                configure(app);
            });

        await VerifyDocumentAsync(application, representation);
    }

    private static async Task VerifyDocumentAsync(
        TestApplication application,
        StrongTypeOpenApiRepresentation representation)
    {
        var openApiJson = await application.Client.GetStringAsync(
            "/openapi/v1.json",
            TestContext.Current.CancellationToken);

        await VerifyJson(openApiJson).UseParameters(representation);
    }

    [StrongType<Guid>]
    public sealed partial class UserId;

    [StrongType<int>]
    public sealed partial class Age;
    
    [StrongType<string>]
    public sealed partial class Tag;

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
