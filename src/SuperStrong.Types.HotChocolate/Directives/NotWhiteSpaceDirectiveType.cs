using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class NotWhiteSpaceDirectiveType : DirectiveType<NotWhiteSpaceDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<NotWhiteSpaceDirective> descriptor)
    {
        descriptor.Name("notWhiteSpace");

        descriptor.Location(DirectiveLocation.Scalar);
    }
}
