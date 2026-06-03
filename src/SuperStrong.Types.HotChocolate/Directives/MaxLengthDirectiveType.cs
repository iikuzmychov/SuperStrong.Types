using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class MaxLengthDirectiveType : DirectiveType<MaxLengthDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<MaxLengthDirective> descriptor)
    {
        descriptor.Name("maxLength");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Argument(directive => directive.Value);
    }
}
