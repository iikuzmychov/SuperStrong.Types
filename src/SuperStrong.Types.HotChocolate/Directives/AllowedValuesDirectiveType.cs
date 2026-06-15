using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class AllowedValuesDirectiveType : DirectiveType<AllowedValuesDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<AllowedValuesDirective> descriptor)
    {
        descriptor.Name("allowedValues");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Repeatable();

        descriptor.Argument(directive => directive.Values).Type<NonNullType<ListType<NonNullType<AnyType>>>>();
    }
}
