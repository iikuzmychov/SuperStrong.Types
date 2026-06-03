using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class MinLengthDirectiveType : DirectiveType<MinLengthDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<MinLengthDirective> descriptor)
    {
        descriptor.Name("minLength");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Argument(directive => directive.Value);
    }
}
