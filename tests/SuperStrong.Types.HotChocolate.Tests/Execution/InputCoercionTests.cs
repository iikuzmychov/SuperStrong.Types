using HotChocolate.Execution;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.HotChocolate.Tests.Execution;

public sealed class InputCoercionTests
{
    public sealed class OrderInput
    {
        public StrongGuid Id { get; set; } = null!;
        public StrongString Name { get; set; } = null!;
    }

    public sealed class Query
    {
        public int Length(StrongString input)
        {
            return input.AsPrimitive().Length;
        }

        public int Sum(List<StrongInt> input)
        {
            return input.Sum(value => value.AsPrimitive());
        }

        public string Describe(OrderInput input)
        {
            return $"{input.Id.AsPrimitive()}/{input.Name.AsPrimitive()}";
        }
    }

    private static readonly Lazy<Task<IRequestExecutor>> _executor = new(GraphQLTest.CreateExecutorAsync<Query>);

    [Fact]
    public async Task A_literal_argument_is_coerced_into_the_strong_type()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("""{ length(input: "alice") }""", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(5, GraphQLTest.GetData(response, "length").GetInt32());
    }

    [Fact]
    public async Task A_variable_argument_is_coerced_into_the_strong_type()
    {
        var executor = await _executor.Value;

        var request = OperationRequestBuilder.New()
            .SetDocument("query($input: StrongString!) { length(input: $input) }")
            .SetVariableValues(new Dictionary<string, object?> { ["input"] = "alice" })
            .Build();

        var result = await executor.ExecuteAsync(request, TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(5, GraphQLTest.GetData(response, "length").GetInt32());
    }

    [Fact]
    public async Task A_list_of_strong_types_is_coerced_element_wise()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("{ sum(input: [1, 2, 3]) }", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(6, GraphQLTest.GetData(response, "sum").GetInt32());
    }

    [Fact]
    public async Task A_strong_type_inside_an_input_object_literal_is_coerced()
    {
        var executor = await _executor.Value;
        var id = Guid.Parse("12345678-1234-1234-1234-1234567890ab");

        var result = await executor.ExecuteAsync(
            $$"""{ describe(input: { id: "{{id}}", name: "alice" }) }""",
            TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal($"{id}/alice", GraphQLTest.GetData(response, "describe").GetString());
    }

    [Fact]
    public async Task A_strong_type_inside_an_input_object_variable_is_coerced()
    {
        var executor = await _executor.Value;
        var id = Guid.Parse("12345678-1234-1234-1234-1234567890ab");

        var input = new Dictionary<string, object?>
        {
            ["id"] = id.ToString(),
            ["name"] = "alice",
        };

        var request = OperationRequestBuilder.New()
            .SetDocument("query($input: OrderInput!) { describe(input: $input) }")
            .SetVariableValues(new Dictionary<string, object?> { ["input"] = input })
            .Build();

        var result = await executor.ExecuteAsync(request, TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal($"{id}/alice", GraphQLTest.GetData(response, "describe").GetString());
    }
}
