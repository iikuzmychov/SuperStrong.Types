using HotChocolate.Types;
using SuperStrong.Types.HotChocolate.Internal;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.HotChocolate;

public sealed class StrongTypeHotChocolateOptionsBuilder
{
    private readonly StrongTypeHotChocolateOptions _options;

    internal StrongTypeHotChocolateOptionsBuilder(StrongTypeHotChocolateOptions options)
    {
        _options = options;
    }

    public StrongTypeHotChocolateOptionsBuilder AddValidatorAdapter<TValidator, TPrimitive, TDirective>(
        IStrongTypeValidatorDirectiveAdapter<TValidator, TPrimitive, TDirective> adapter)
        where TValidator : StrongTypeValidator<TPrimitive>
        where TPrimitive : notnull
    {
        _options.RegisterAdapter(adapter);

        return this;
    }

    public StrongTypeHotChocolateOptionsBuilder AddDirectiveType<TDirectiveType>()
        where TDirectiveType : DirectiveType
    {
        _options.RegisterDirectiveType(typeof(TDirectiveType));

        return this;
    }
}
