namespace SuperStrong.Types.Tests.Converters;

public sealed class GuidNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples, StrongGuid.InvalidPrimitiveSamples>;
