using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators;

internal static class Known
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
        public static readonly TypeName SuperStrong_Types_StrongTypeParsableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeParsableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeSpanParsableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeSpanParsableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeUtf8SpanParsableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeUtf8SpanParsableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeFormattableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeFormattableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeSpanFormattableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeSpanFormattableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeUtf8SpanFormattableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeUtf8SpanFormattableFeatureAttribute");
        public static readonly TypeName SuperStrong_Types_StrongTypeComparableFeatureAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeComparableFeatureAttribute");

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
