using HotChocolate.Execution;
using HotChocolate.Transport.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Tests;

internal static class GraphQLTest
{
    private static readonly ConcurrentDictionary<(Type QueryType, StrongTypeGraphQLRepresentation Representation), Lazy<Task<IRequestExecutor>>> _executors = new();

    public static Task<IRequestExecutor> GetExecutorAsync<TQuery>(StrongTypeGraphQLRepresentation representation)
        where TQuery : class
    {
        return _executors
            .GetOrAdd(
                (typeof(TQuery), representation),
                _ => new Lazy<Task<IRequestExecutor>>(() => CreateExecutorAsync<TQuery>(representation)))
            .Value;
    }

    private static async Task<IRequestExecutor> CreateExecutorAsync<TQuery>(StrongTypeGraphQLRepresentation representation)
        where TQuery : class
    {
        return await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<TQuery>()
            .AddStrongTypes(representation)
            .BuildRequestExecutorAsync();
    }

    public static JsonDocument ToJsonDocument(IExecutionResult result)
    {
        var operationResult = (OperationResult)result.ExpectOperationResult();
        var buffer = new ArrayBufferWriter<byte>();

        JsonResultFormatter.Default.Format(operationResult, buffer);

        return JsonDocument.Parse(buffer.WrittenMemory.ToArray());
    }

    public static JsonElement GetData(JsonDocument response, string field)
    {
        if (response.RootElement.TryGetProperty("errors", out var errors))
        {
            Assert.Fail(errors.GetRawText());
        }

        return response.RootElement.GetProperty("data").GetProperty(field);
    }

    public static JsonElement GetErrors(JsonDocument response)
    {
        Assert.True(
            response.RootElement.TryGetProperty("errors", out var errors),
            "Expected the response to contain errors, but it has none.");

        return errors;
    }
}
