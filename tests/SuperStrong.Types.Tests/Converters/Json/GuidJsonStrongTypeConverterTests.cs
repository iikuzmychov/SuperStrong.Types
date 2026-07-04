namespace SuperStrong.Types.Tests.Converters;

public sealed class GuidJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples, StrongGuid.InvalidPrimitiveSamples>;
