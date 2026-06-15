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

        builder.AddDirectiveType<NotWhiteSpaceDirectiveType>();
        builder.AddValidatorAdapter(new NotWhiteSpaceValidatorAdapter());

        builder.AddDirectiveType<RegexDirectiveType>();
        builder.AddValidatorAdapter(new RegexValidatorAdapter());

        builder.AddDirectiveType<LowerInvariantDirectiveType>();
        builder.AddValidatorAdapter(new LowerInvariantValidatorAdapter());

        builder.AddDirectiveType<UpperInvariantDirectiveType>();
        builder.AddValidatorAdapter(new UpperInvariantValidatorAdapter());

        builder.AddDirectiveType<MinValueDirectiveType>();
        builder.AddValidatorAdapterFactory(new MinValueValidatorAdapterFactory());

        builder.AddDirectiveType<MaxValueDirectiveType>();
        builder.AddValidatorAdapterFactory(new MaxValueValidatorAdapterFactory());

        builder.AddDirectiveType<AllowedValuesDirectiveType>();
        builder.AddValidatorAdapterFactory(new AllowedValuesValidatorAdapterFactory());

        builder.AddDirectiveType<ForbiddenValuesDirectiveType>();
        builder.AddValidatorAdapterFactory(new ForbiddenValuesValidatorAdapterFactory());

        return builder;
    }
}
