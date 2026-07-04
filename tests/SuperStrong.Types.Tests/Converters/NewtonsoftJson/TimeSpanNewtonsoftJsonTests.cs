namespace SuperStrong.Types.Tests.Converters;

public sealed class TimeSpanNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples, StrongTimeSpan.InvalidPrimitiveSamples>;
