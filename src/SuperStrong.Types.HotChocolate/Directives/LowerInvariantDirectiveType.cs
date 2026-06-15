using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class LowerInvariantDirectiveType : DirectiveType<LowerInvariantDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<LowerInvariantDirective> descriptor)
    {
        descriptor.Name("lowerInvariant");

        descriptor.Location(DirectiveLocation.Scalar);
    }
}
