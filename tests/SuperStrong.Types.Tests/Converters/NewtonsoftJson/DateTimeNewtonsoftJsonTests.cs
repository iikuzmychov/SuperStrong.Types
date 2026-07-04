namespace SuperStrong.Types.Tests.Converters;

public sealed class DateTimeNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples, StrongDateTime.InvalidPrimitiveSamples>;
