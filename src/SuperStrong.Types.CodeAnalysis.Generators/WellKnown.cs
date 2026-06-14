using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators;

internal static class WellKnown
{
    internal static class Namespaces
    {
        public const string SuperStrong_Types = "SuperStrong.Types";
        public const string SuperStrong_Types_Converters = "SuperStrong.Types.Converters";

        public const string System = "System";
        public const string System_Numerics = "System.Numerics";
        public const string System_CodeDom_Compiler = "System.CodeDom.Compiler";
        public const string System_ComponentModel = "System.ComponentModel";
        public const string System_Diagnostics = "System.Diagnostics";
        public const string System_Diagnostics_CodeAnalysis = "System.Diagnostics.CodeAnalysis";
        public const string System_Text_Json_Serialization = "System.Text.Json.Serialization";
    }

    internal static class Types
    {
        public static readonly TypeName SuperStrong_Types_StrongTypeAttribute = new(Namespaces.SuperStrong_Types, "StrongTypeAttribute");
        public static readonly TypeName SuperStrong_Types_IStrongType = new(Namespaces.SuperStrong_Types, "IStrongType");
        public static readonly TypeName SuperStrong_Types_StrongType = new(Namespaces.SuperStrong_Types, "StrongType");
        public static readonly TypeName SuperStrong_Types_StrongTypeDefinition = new(Namespaces.SuperStrong_Types, "StrongTypeDefinition");
        public static readonly TypeName SuperStrong_Types_IStrongTypeTemplate = new(Namespaces.SuperStrong_Types, "IStrongTypeTemplate");
        public static readonly TypeName SuperStrong_Types_Converters_StrongTypeConverter = new(Namespaces.SuperStrong_Types_Converters, "StrongTypeConverter");
        public static readonly TypeName SuperStrong_Types_Converters_JsonStrongTypeConverter = new(Namespaces.SuperStrong_Types_Converters, "JsonStrongTypeConverter");

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
        public static readonly TypeName System_IConvertible = new(Namespaces.System, "IConvertible");
        public static readonly TypeName System_TypeCode = new(Namespaces.System, "TypeCode");
        public static readonly TypeName System_Type = new(Namespaces.System, "Type");
        public static readonly TypeName System_Numerics_IEqualityOperators = new(Namespaces.System_Numerics, "IEqualityOperators");
        public static readonly TypeName System_Numerics_IComparisonOperators = new(Namespaces.System_Numerics, "IComparisonOperators");
        public static readonly TypeName System_CodeDom_Compiler_GeneratedCodeAttribute = new(Namespaces.System_CodeDom_Compiler, "GeneratedCodeAttribute");
        public static readonly TypeName System_ComponentModel_TypeConverterAttribute = new(Namespaces.System_ComponentModel, "TypeConverterAttribute");
        public static readonly TypeName System_Diagnostics_DebuggerDisplayAttribute = new(Namespaces.System_Diagnostics, "DebuggerDisplayAttribute");
        public static readonly TypeName System_Text_Json_Serialization_JsonConverterAttribute = new(Namespaces.System_Text_Json_Serialization, "JsonConverterAttribute");
        public static readonly TypeName System_Diagnostics_CodeAnalysis_NotNullWhenAttribute = new (Namespaces.System_Diagnostics_CodeAnalysis, "NotNullWhenAttribute");
        public static readonly TypeName System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute = new(Namespaces.System_Diagnostics_CodeAnalysis, "MaybeNullWhenAttribute");
    }
}
