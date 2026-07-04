namespace SuperStrong.Types.Tests.Converters;

public sealed class DecimalJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples, StrongDecimal.InvalidPrimitiveSamples>;
