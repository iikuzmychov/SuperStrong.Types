namespace SuperStrong.Types.Tests.Converters;

public sealed class DateTimeOffsetJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples, StrongDateTimeOffset.InvalidPrimitiveSamples>;
