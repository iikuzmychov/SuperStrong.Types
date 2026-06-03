using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

public sealed class PrimitiveDirectiveType : DirectiveType<PrimitiveDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<PrimitiveDirective> descriptor)
    {
        descriptor.Name("primitive");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Argument(directive => directive.Type);
    }
}
