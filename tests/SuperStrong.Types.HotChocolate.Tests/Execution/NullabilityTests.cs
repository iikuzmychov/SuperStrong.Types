using HotChocolate.Execution;
using SuperStrong.Types.Tests;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Tests.Execution;

public sealed class NullabilityTests
{
    public sealed class Query
    {
        public StrongString? Missing()
        {
            return null;
        }

        public int Length(StrongString? input)
        {
            return input?.AsPrimitive().Length ?? -1;
        }
    }

    private static readonly Lazy<Task<IRequestExecutor>> _executor = new(GraphQLTest.CreateExecutorAsync<Query>);

    [Fact]
    public async Task A_null_strong_type_output_is_serialized_as_null()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("{ missing }", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(JsonValueKind.Null, GraphQLTest.GetData(response, "missing").ValueKind);
    }

    [Fact]
    public async Task A_null_literal_argument_is_coerced_to_null()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("{ length(input: null) }", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(-1, GraphQLTest.GetData(response, "length").GetInt32());
    }

    [Fact]
    public async Task An_omitted_optional_argument_is_coerced_to_null()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("{ length }", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(-1, GraphQLTest.GetData(response, "length").GetInt32());
    }
}
