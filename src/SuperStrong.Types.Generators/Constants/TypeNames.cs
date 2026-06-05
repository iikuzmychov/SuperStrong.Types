using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.Constants;

internal static class TypeNames
{
    public static readonly TypeName StrongTypeAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeAttribute");
    public static readonly TypeName IStrongType = new(Namespaces.SuperStrong_Types, "IStrongType");
    public static readonly TypeName StrongType = new(Namespaces.SuperStrong_Types, "StrongType");
    public static readonly TypeName IHasStrongTypeDefinition = new(Namespaces.SuperStrong_Types, "IHasStrongTypeDefinition");
    public static readonly TypeName StrongTypeDefinition = new(Namespaces.SuperStrong_Types, "StrongTypeDefinition");
    public static readonly TypeName IStrongTypeTemplate = new(Namespaces.SuperStrong_Types, "IStrongTypeTemplate");
}
