namespace SuperStrong.Types.Tests.Converters;

public sealed class TimeOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples, StrongTimeOnly.InvalidPrimitiveSamples>;
