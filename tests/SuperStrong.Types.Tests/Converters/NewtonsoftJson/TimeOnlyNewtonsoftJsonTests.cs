namespace SuperStrong.Types.Tests.Converters;

public sealed class TimeOnlyNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples, StrongTimeOnly.InvalidPrimitiveSamples>;
