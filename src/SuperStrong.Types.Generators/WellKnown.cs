using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators;

internal static class WellKnown
{
    internal static class Namespaces
    {
        public const string SuperStrong_Types = "SuperStrong.Types";

        public const string System = "System";
        public const string System_Numerics = "System.Numerics";
        public const string System_Diagnostics_CodeAnalysis = "System.Diagnostics.CodeAnalysis";
    }

    internal static class Types
    {
        public static readonly TypeName SuperStrong_Types_StrongTypeAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeAttribute");
        public static readonly TypeName SuperStrong_Types_IStrongType = new(Namespaces.SuperStrong_Types, "IStrongType");
        public static readonly TypeName SuperStrong_Types_StrongType = new(Namespaces.SuperStrong_Types, "StrongType");
        public static readonly TypeName SuperStrong_Types_IHasStrongTypeDefinition = new(Namespaces.SuperStrong_Types, "IHasStrongTypeDefinition");
        public static readonly TypeName SuperStrong_Types_StrongTypeDefinition = new(Namespaces.SuperStrong_Types, "StrongTypeDefinition");
        public static readonly TypeName SuperStrong_Types_IStrongTypeTemplate = new(Namespaces.SuperStrong_Types, "IStrongTypeTemplate");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_ParsableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+ParsableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_SpanParsableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+SpanParsableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_Utf8SpanParsableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+Utf8SpanParsableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_FormattableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+FormattableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_SpanFormattableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+SpanFormattableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_Utf8SpanFormattableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+Utf8SpanFormattableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Lifting_ComparableAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Lifting+ComparableAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFeatures_Equality_PartialDefinitionAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFeatures+Equality+PartialDefinitionAttribute");

        public static readonly TypeName System_Span = new(Namespaces.System, "Span");
        public static readonly TypeName System_ReadOnlySpan = new(Namespaces.System, "ReadOnlySpan");
        public static readonly TypeName System_IFormatProvider = new(Namespaces.System, "IFormatProvider");
        public static readonly TypeName System_IEquatable = new(Namespaces.System, "IEquatable");
        public static readonly TypeName System_IParsable = new(Namespaces.System, "IParsable");
        public static readonly TypeName System_ISpanParsable = new(Namespaces.System, "ISpanParsable");
        public static readonly TypeName System_IUtf8SpanParsable = new(Namespaces.System, "IUtf8SpanParsable");
        public static readonly TypeName System_IFormattable = new(Namespaces.System, "IFormattable");
        public static readonly TypeName System_ISpanFormattable = new(Namespaces.System, "ISpanFormattable");
        public static readonly TypeName System_IUtf8SpanFormattable = new(Namespaces.System, "IUtf8SpanFormattable");
        public static readonly TypeName System_IComparable = new(Namespaces.System, "IComparable");
        public static readonly TypeName System_Numerics_IEqualityOperators = new(Namespaces.System_Numerics, "IEqualityOperators");
        public static readonly TypeName System_Diagnostics_CodeAnalysis_NotNullWhenAttribute = new (Namespaces.System_Diagnostics_CodeAnalysis, "NotNullWhenAttribute");
        public static readonly TypeName System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute = new(Namespaces.System_Diagnostics_CodeAnalysis, "MaybeNullWhenAttribute");
    }
}
