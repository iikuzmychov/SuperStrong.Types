using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Transport.Formatters;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Text;

namespace SuperStrong.Types.HotChocolate.Tests;

public sealed class StrongScalarTypeTests
{
    [Fact]
    public async Task Schema_exposes_strong_type_as_scalar_with_primitive_directive()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar Username", schema);
        Assert.Contains("@primitive(type: \"String\")", schema);
    }

    [Fact]
    public async Task Input_literal_is_coerced_into_the_strong_type()
    {
        var executor = await BuildExecutorAsync();
        var scalar = executor.Schema.Types.GetType<ScalarType>("Username");

        var runtimeValue = scalar.CoerceInputLiteral(new StringValueNode("alice"));

        var username = Assert.IsType<Username>(runtimeValue);
        Assert.Equal("alice", username.AsPrimitive());
    }

    [Fact]
    public async Task Runtime_value_is_converted_back_to_a_literal()
    {
        var executor = await BuildExecutorAsync();
        var scalar = executor.Schema.Types.GetType<ScalarType>("Username");

        var literal = scalar.ValueToLiteral(Username.From("alice"));

        var stringLiteral = Assert.IsType<StringValueNode>(literal);
        Assert.Equal("alice", stringLiteral.Value);
    }

    [Fact]
    public async Task Output_value_is_serialized_through_the_underlying_scalar()
    {
        var executor = await BuildExecutorAsync();

        var result = await executor.ExecuteAsync("{ echo(input: \"alice\") }", TestContext.Current.CancellationToken);
        var json = ToJson(result);

        Assert.DoesNotContain("errors", json);
        Assert.Contains("\"alice\"", json);
    }

    [Fact]
    public async Task Strong_type_reachable_only_through_an_input_object_is_a_scalar_not_an_object()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("input AddressInput", schema);
        Assert.Contains("scalar City", schema);
        Assert.DoesNotContain("type City", schema);
        Assert.DoesNotContain("input City", schema);
    }

    [Fact]
    public async Task Schema_registers_the_primitive_scalar_of_a_Guid_strong_type()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar OrderId", schema);
        Assert.Contains("@primitive(type: \"UUID\")", schema);
    }

    [Fact]
    public async Task Guid_strong_type_output_value_is_serialized_through_the_UUID_scalar()
    {
        var executor = await BuildExecutorAsync();
        var id = Guid.NewGuid();

        var result = await executor.ExecuteAsync($$"""{ echoId(input: "{{id}}") }""", TestContext.Current.CancellationToken);
        var json = ToJson(result);

        Assert.DoesNotContain("errors", json);
        Assert.Contains($"\"{id}\"", json);
    }

    [Fact]
    public async Task Input_variable_is_coerced_into_the_strong_type()
    {
        var executor = await BuildExecutorAsync();

        var request = OperationRequestBuilder.New()
            .SetDocument("query($input: Username!) { length(input: $input) }")
            .SetVariableValues(new Dictionary<string, object?> { ["input"] = "alice" })
            .Build();

        var result = await executor.ExecuteAsync(request, TestContext.Current.CancellationToken);
        var json = ToJson(result);

        Assert.DoesNotContain("errors", json);
        Assert.Contains("\"length\":5", json);
    }

    private static async Task<IRequestExecutor> BuildExecutorAsync()
    {
        return await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<Query>()
            .AddStrongTypes()
            .BuildRequestExecutorAsync(cancellationToken: TestContext.Current.CancellationToken);
    }

    private static string ToJson(IExecutionResult result)
    {
        var operationResult = (OperationResult)result.ExpectOperationResult();
        var buffer = new ArrayBufferWriter<byte>();

        JsonResultFormatter.Default.Format(operationResult, buffer);

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }
}

[StrongType<string>]
public sealed partial class Username
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>();
}

[StrongType<string>]
public sealed partial class City
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>();
}

[StrongType<Guid>]
public sealed partial class OrderId
{
    public static StrongTypeDefinition<Guid> Definition { get; } = StrongType.Define<Guid>();
}

public sealed class AddressInput
{
    public City City { get; set; } = null!;
}

public sealed class Query
{
    public Username Echo(Username input)
    {
        return input;
    }

    public int Length(Username input)
    {
        return input.AsPrimitive().Length;
    }

    public string Describe(AddressInput input)
    {
        return input.City.AsPrimitive();
    }

    public OrderId EchoId(OrderId input)
    {
        return input;
    }
}
