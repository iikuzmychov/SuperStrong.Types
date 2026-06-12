namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed record HasBaseTypeInfo(string TypeFullName, string BaseTypeFullName, LocationInfo? Location);
