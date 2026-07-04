namespace SuperStrong.Types.Tests.Converters;

public sealed class BoolNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples, StrongBool.InvalidPrimitiveSamples>;
