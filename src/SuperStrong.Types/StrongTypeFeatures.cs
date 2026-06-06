namespace SuperStrong.Types;

public static class StrongTypeFeatures
{
    public static class Lifting
    {
        public sealed class ParsableAttribute : StrongTypeFeatureAttribute;
        public sealed class SpanParsableAttribute : StrongTypeFeatureAttribute;
        public sealed class Utf8SpanParsableAttribute : StrongTypeFeatureAttribute;
        public sealed class FormattableAttribute : StrongTypeFeatureAttribute;
        public sealed class SpanFormattableAttribute : StrongTypeFeatureAttribute;
        public sealed class Utf8SpanFormattableAttribute : StrongTypeFeatureAttribute;
        public sealed class ComparableAttribute : StrongTypeFeatureAttribute;
    }
}
