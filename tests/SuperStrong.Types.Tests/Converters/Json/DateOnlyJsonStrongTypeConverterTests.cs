namespace SuperStrong.Types.Tests.Converters;

public sealed class DateOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples, StrongDateOnly.InvalidPrimitiveSamples>;
