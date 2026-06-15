using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class RegexDirectiveType : DirectiveType<RegexDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<RegexDirective> descriptor)
    {
        descriptor.Name("regex");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Repeatable();

        descriptor.Argument(directive => directive.Pattern);

        descriptor.Argument(directive => directive.Options);
    }
}
