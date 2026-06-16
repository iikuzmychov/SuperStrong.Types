using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Transport.Formatters;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using System.Text;
using System.Text.RegularExpressions;

namespace SuperStrong.Types.HotChocolate.Tests;

public sealed class StrongTypeScalarTypeTests
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
    public async Task Validators_are_emitted_as_directives_on_the_scalar()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar Slug", schema);
        Assert.Contains("@minLength(value: 3)", schema);
        Assert.Contains("@maxLength(value: 10)", schema);
    }

    [Fact]
    public async Task String_validators_are_emitted_as_directives_on_the_scalar()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("@notWhiteSpace", schema);
        Assert.Contains("@lowerInvariant", schema);
        Assert.Contains("@regex(pattern: \"^[a-z]+$\", options: \"IgnoreCase\")", schema);
        Assert.Contains("@upperInvariant", schema);
    }

    [Fact]
    public async Task Numeric_validators_are_emitted_as_directives_on_the_scalar()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar Quantity", schema);
        Assert.Contains("@minValue(value: 1, isExclusive: true)", schema);
        Assert.Contains("@maxValue(value: 100, isExclusive: false)", schema);
    }

    [Fact]
    public async Task Set_validators_are_emitted_as_directives_on_the_scalar()
    {
        var executor = await BuildExecutorAsync();

        var schema = executor.Schema.ToString();

        Assert.Contains("@allowedValues(", schema);
        Assert.Contains("\"EUR\"", schema);
        Assert.Contains("\"USD\"", schema);
        Assert.Contains("@allowedValues(values: [\"EUR\", \"USD\"])", schema);
        Assert.Contains("@forbiddenValues(values: [\"admin\", \"root\"])", schema);
        Assert.Contains("@allowedValues(values: [1, 2, 3])", schema);
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
public sealed partial class Slug
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().HasMinLength(3).HasMaxLength(10);
}

[StrongType<string>]
public sealed partial class Handle
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().IsNotWhiteSpace().IsLowerInvariant().MatchesRegex("^[a-z]+$", RegexOptions.IgnoreCase);
}

[StrongType<string>]
public sealed partial class Code
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().IsUpperInvariant();
}

[StrongType<int>]
public sealed partial class Quantity
{
    public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>().HasMinValue(1, isExclusive: true).HasMaxValue(100);
}

[StrongType<string>]
public sealed partial class Currency
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().IsOneOf("EUR", "USD");
}

[StrongType<string>]
public sealed partial class Reserved
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().IsNotOneOf("admin", "root");
}

[StrongType<int>]
public sealed partial class Rating
{
    public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>().IsOneOf(1, 2, 3);
}

[StrongType<string>]
public sealed partial class City
{
    public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>();
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

    public Slug Slug(Slug input)
    {
        return input;
    }

    public Handle Handle(Handle input)
    {
        return input;
    }

    public Code Code(Code input)
    {
        return input;
    }

    public Quantity Quantity(Quantity input)
    {
        return input;
    }

    public Currency Currency(Currency input)
    {
        return input;
    }

    public Reserved Reserved(Reserved input)
    {
        return input;
    }

    public Rating Rating(Rating input)
    {
        return input;
    }

    public string Describe(AddressInput input)
    {
        return input.City.AsPrimitive();
    }
}
