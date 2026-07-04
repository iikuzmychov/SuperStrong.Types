namespace SuperStrong.Types.Tests.Converters;

public sealed class DecimalNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples, StrongDecimal.InvalidPrimitiveSamples>;
