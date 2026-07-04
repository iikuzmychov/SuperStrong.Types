namespace SuperStrong.Types.Tests.Converters;

public sealed class TimeSpanJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples, StrongTimeSpan.InvalidPrimitiveSamples>;
