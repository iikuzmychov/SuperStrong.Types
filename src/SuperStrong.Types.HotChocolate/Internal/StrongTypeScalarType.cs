using HotChocolate.Configuration;
using HotChocolate.Features;
using HotChocolate.Internal;
using HotChocolate.Language;
using HotChocolate.Text.Json;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Configurations;
using SuperStrong.Types.HotChocolate.Directives;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongTypeScalarType<TStrongType, TPrimitive> : ScalarType<TStrongType>
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    private ScalarType _primitive = null!;

    public StrongTypeScalarType() : base(typeof(TStrongType).Name, BindingBehavior.Implicit)
    {
    }

    public override ScalarSerializationType SerializationType => _primitive.SerializationType;

    protected override void OnRegisterDependencies(
        ITypeDiscoveryContext context,
        ScalarTypeConfiguration configuration)
    {
        base.OnRegisterDependencies(context, configuration);

        var primitiveRef = context.TypeInspector.GetTypeRef(typeof(TPrimitive));
        configuration.Dependencies.Add(new TypeDependency(primitiveRef, TypeDependencyFulfilled.Completed));

        var directiveRef = context.TypeInspector.GetTypeRef(typeof(PrimitiveDirectiveType));
        configuration.Dependencies.Add(new TypeDependency(directiveRef, TypeDependencyFulfilled.Completed));
    }

    protected override void OnCompleteType(
        ITypeCompletionContext context,
        ScalarTypeConfiguration configuration)
    {
        base.OnCompleteType(context, configuration);

        var primitiveRef = context.TypeInspector.GetTypeRef(typeof(TPrimitive));
        _primitive = (ScalarType)context.GetType<IType>(primitiveRef).NamedType();

        configuration.AddDirective(PrimitiveDirective.From(_primitive.Name), context.TypeInspector);
    }

    public override object CoerceInputLiteral(IValueNode valueLiteral)
    {
        ArgumentNullException.ThrowIfNull(valueLiteral);

        var primitive = (TPrimitive)_primitive.CoerceInputLiteral(valueLiteral);

        return TStrongType.From(primitive);
    }

    public override object CoerceInputValue(JsonElement inputValue, IFeatureProvider context)
    {
        var primitive = (TPrimitive)_primitive.CoerceInputValue(inputValue, context);

        return TStrongType.From(primitive);
    }

    protected override IValueNode OnValueToLiteral(TStrongType runtimeValue)
    {
        return _primitive.ValueToLiteral(runtimeValue.AsPrimitive());
    }

    protected override void OnCoerceOutputValue(TStrongType runtimeValue, ResultElement resultValue)
    {
        _primitive.CoerceOutputValue(runtimeValue.AsPrimitive(), resultValue);
    }
}
