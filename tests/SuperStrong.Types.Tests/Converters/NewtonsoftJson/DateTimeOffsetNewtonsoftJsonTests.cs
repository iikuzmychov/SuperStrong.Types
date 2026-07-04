namespace SuperStrong.Types.Tests.Converters;

public sealed class DateTimeOffsetNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples, StrongDateTimeOffset.InvalidPrimitiveSamples>;
