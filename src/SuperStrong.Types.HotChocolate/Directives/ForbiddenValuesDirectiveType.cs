using HotChocolate.Types;

namespace SuperStrong.Types.HotChocolate.Directives;

internal sealed class ForbiddenValuesDirectiveType : DirectiveType<ForbiddenValuesDirective>
{
    protected override void Configure(IDirectiveTypeDescriptor<ForbiddenValuesDirective> descriptor)
    {
        descriptor.Name("forbiddenValues");

        descriptor.Location(DirectiveLocation.Scalar);

        descriptor.Repeatable();

        descriptor.Argument(directive => directive.Values).Type<NonNullType<ListType<NonNullType<AnyType>>>>();
    }
}
