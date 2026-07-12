using SuperStrong.Types.Tests;

namespace SuperStrong.Types.HotChocolate.Tests.Schema;

public sealed class AddressInput
{
    public StrongString City { get; set; } = null!;
}

public sealed class Query
{
    public int Value(StrongInt input)
    {
        return input.AsPrimitive();
    }

    public bool Exists(StrongGuid id)
    {
        return id.AsPrimitive() != Guid.Empty;
    }

    public string Describe(AddressInput input)
    {
        return input.City.AsPrimitive();
    }
}

public sealed class StrongTypeSchemaTests
{
    [Fact]
    public async Task A_strong_type_is_exposed_as_a_scalar_with_the_strong_type_directive()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.StrongType);

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar StrongInt", schema);
        Assert.Contains("@strongType(primitiveType: \"Int\")", schema);
    }

    [Fact]
    public async Task A_Guid_strong_type_maps_to_the_UUID_scalar()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.StrongType);

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar StrongGuid", schema);
        Assert.Contains("@strongType(primitiveType: \"UUID\")", schema);
    }

    [Fact]
    public async Task A_string_strong_type_maps_to_the_String_scalar()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.StrongType);

        var schema = executor.Schema.ToString();

        Assert.Contains("scalar StrongString", schema);
        Assert.Contains("@strongType(primitiveType: \"String\")", schema);
    }

    [Fact]
    public async Task A_strong_type_reachable_only_through_an_input_object_is_a_scalar_not_an_object()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.StrongType);

        var schema = executor.Schema.ToString();

        Assert.Contains("input AddressInput", schema);
        Assert.Contains("scalar StrongString", schema);
        Assert.DoesNotContain("type StrongString", schema);
        Assert.DoesNotContain("input StrongString", schema);
    }
}

public sealed class PrimitiveTypeSchemaTests
{
    [Fact]
    public async Task A_strong_type_is_exposed_as_its_primitive_scalar()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.PrimitiveType);

        var schema = executor.Schema.ToString();

        Assert.Contains("value(input: Int!): Int!", schema);
        Assert.DoesNotContain("StrongInt", schema);
        Assert.DoesNotContain("@strongType", schema);
    }

    [Fact]
    public async Task A_Guid_strong_type_maps_to_the_UUID_scalar()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.PrimitiveType);

        var schema = executor.Schema.ToString();

        Assert.Contains("exists(id: UUID!): Boolean!", schema);
        Assert.DoesNotContain("StrongGuid", schema);
    }

    [Fact]
    public async Task A_strong_type_inside_an_input_object_is_exposed_as_its_primitive_scalar()
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(StrongTypeGraphQLRepresentation.PrimitiveType);

        var schema = executor.Schema.ToString();

        Assert.Contains("input AddressInput", schema);
        Assert.Contains("city: String!", schema);
        Assert.DoesNotContain("StrongString", schema);
    }
}
