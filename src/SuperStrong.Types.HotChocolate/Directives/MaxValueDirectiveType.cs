using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class MaxValueDirectiveType : DirectiveType<MaxValueDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<MaxValueDirective> descriptor)
    {
        descriptor.Name("maxValue");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Repeatable();

        descriptor.Argument(directive => directive.Value).Type<NonNullType<AnyType>>();

        descriptor.Argument(directive => directive.IsExclusive);
    }
}
