using SuperStrong.Types.HotChocolate.Adapters;
using SuperStrong.Types.HotChocolate.Directives;

namespace SuperStrong.Types.HotChocolate;

public static class StrongTypeHotChocolateOptionsBuilderExtensions
{
    public static StrongTypeHotChocolateOptionsBuilder AddDefaults(this StrongTypeHotChocolateOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddDirectiveType<PrimitiveDirectiveType>();

        builder.AddDirectiveType<MaxLengthDirectiveType>();
        builder.AddValidatorAdapter(new MaxLengthValidatorAdapter());

        builder.AddDirectiveType<MinLengthDirectiveType>();
        builder.AddValidatorAdapter(new MinLengthValidatorAdapter());

        return builder;
    }
}
