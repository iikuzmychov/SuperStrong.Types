namespace SuperStrong.Types.Tests.Converters;

public sealed class DateOnlyNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples, StrongDateOnly.InvalidPrimitiveSamples>;
