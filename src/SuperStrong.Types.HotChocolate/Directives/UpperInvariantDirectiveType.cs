using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class UpperInvariantDirectiveType : DirectiveType<UpperInvariantDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<UpperInvariantDirective> descriptor)
    {
        descriptor.Name("upperInvariant");

        descriptor.Location(DirectiveLocation.Scalar);
    }
}
