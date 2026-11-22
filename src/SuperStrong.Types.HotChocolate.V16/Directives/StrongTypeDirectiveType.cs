using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

public sealed class StrongTypeDirectiveType : DirectiveType<StrongTypeDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<StrongTypeDirective> descriptor)
    {
        descriptor.Name("strongType");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Argument(directive => directive.PrimitiveType);
    }
}
