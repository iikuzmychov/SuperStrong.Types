using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class MinValueDirectiveType : DirectiveType<MinValueDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<MinValueDirective> descriptor)
    {
        descriptor.Name("minValue");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Repeatable();

        descriptor.Argument(directive => directive.Value).Type<NonNullType<AnyType>>();

        descriptor.Argument(directive => directive.IsExclusive);
    }
}
