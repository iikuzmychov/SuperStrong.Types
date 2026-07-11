using HotChocolate.Execution;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.HotChocolate.Tests.Execution;

public sealed class ValidationTests
{
    public sealed class Query
    {
        public int Length(StrongString input)
        {
            return input.AsPrimitive().Length;
        }

        public int Value(StrongInt input)
        {
            return input.AsPrimitive();
        }
    }

    private static readonly Lazy<Task<IRequestExecutor>> _executor = new(GraphQLTest.CreateExecutorAsync<Query>);

    [Fact]
    public async Task A_valid_literal_passes_validation_and_the_query_executes()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync("""{ length(input: "alice") }""", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        Assert.Equal(5, GraphQLTest.GetData(response, "length").GetInt32());
    }

    [Fact]
    public async Task An_invalid_string_literal_produces_an_error_instead_of_data()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync(
            $$"""{ length(input: "{{StrongString.ForbiddenValue}}") }""",
            TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        GraphQLTest.GetErrors(response);
    }

    [Fact]
    public async Task An_invalid_string_variable_produces_an_error_instead_of_data()
    {
        var executor = await _executor.Value;

        var request = OperationRequestBuilder.New()
            .SetDocument("query($input: StrongString!) { length(input: $input) }")
            .SetVariableValues(new Dictionary<string, object?> { ["input"] = StrongString.ForbiddenValue })
            .Build();

        var result = await executor.ExecuteAsync(request, TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        GraphQLTest.GetErrors(response);
    }

    [Fact]
    public async Task An_invalid_int_literal_produces_an_error_instead_of_data()
    {
        var executor = await _executor.Value;

        var result = await executor.ExecuteAsync(
            $"{{ value(input: {StrongInt.ForbiddenValue}) }}",
            TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        GraphQLTest.GetErrors(response);
    }
}
