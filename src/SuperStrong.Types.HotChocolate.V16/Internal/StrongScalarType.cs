using HotChocolate.Configuration;
using HotChocolate.Features;
using HotChocolate.Internal;
using HotChocolate.Language;
using HotChocolate.Text.Json;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Configurations;
using HotChocolate.Utilities;
using SuperStrong.Types.HotChocolate.Directives;
using System.Text.Json;

namespace SuperStrong.Types.HotChocolate.Internal;

internal sealed class StrongScalarType<TStrongType, TPrimitive> : ScalarType<TStrongType>
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    private ScalarType _primitive = null!;

    public StrongScalarType() : base(typeof(TStrongType).Name, BindingBehavior.Implicit)
    {
    }

    public override ScalarSerializationType SerializationType => _primitive.SerializationType;

    protected override void OnRegisterDependencies(
        ITypeDiscoveryContext context,
        ScalarTypeConfiguration configuration)
    {
        base.OnRegisterDependencies(context, configuration);

        var primitiveRef = context.TypeInspector.GetTypeRef(typeof(TPrimitive));
        context.Dependencies.Add(new TypeDependency(primitiveRef, TypeDependencyFulfilled.Completed));

        var directiveRef = context.TypeInspector.GetTypeRef(typeof(StrongTypeDirectiveType));
        context.Dependencies.Add(new TypeDependency(directiveRef, TypeDependencyFulfilled.Completed));
    }

    protected override void OnCompleteType(
        ITypeCompletionContext context,
        ScalarTypeConfiguration configuration)
    {
        base.OnCompleteType(context, configuration);

        var primitiveRef = context.TypeInspector.GetTypeRef(typeof(TPrimitive));
        _primitive = (ScalarType)context.GetType<IType>(primitiveRef).NamedType();

        configuration.AddDirective(StrongTypeDirective.From(_primitive.Name), context.TypeInspector);
    }

    public override bool IsValueCompatible(IValueNode valueLiteral)
    {
        return _primitive.IsValueCompatible(valueLiteral);
    }

    public override bool IsValueCompatible(JsonElement inputValue)
    {
        return _primitive.IsValueCompatible(inputValue);
    }

    public override object CoerceInputLiteral(IValueNode valueLiteral)
    {
        ArgumentNullException.ThrowIfNull(valueLiteral);

        return TStrongType.From(ToPrimitive(_primitive.CoerceInputLiteral(valueLiteral)));
    }

    public override object CoerceInputValue(JsonElement inputValue, IFeatureProvider context)
    {
        return TStrongType.From(ToPrimitive(_primitive.CoerceInputValue(inputValue, context)));
    }

    protected override IValueNode OnValueToLiteral(TStrongType runtimeValue)
    {
        return _primitive.ValueToLiteral(ToScalarRuntimeValue(runtimeValue.AsPrimitive()));
    }

    protected override void OnCoerceOutputValue(TStrongType runtimeValue, ResultElement resultValue)
    {
        _primitive.CoerceOutputValue(ToScalarRuntimeValue(runtimeValue.AsPrimitive()), resultValue);
    }

    private TPrimitive ToPrimitive(object scalarRuntimeValue)
    {
        if (scalarRuntimeValue is TPrimitive primitive ||
            Converter.TryConvert(scalarRuntimeValue, out primitive))
        {
            return primitive;
        }

        throw new LeafCoercionException(
            $"The scalar '{Name}' cannot convert a value of type '{scalarRuntimeValue.GetType()}' to '{typeof(TPrimitive)}'.",
            this);
    }

    private object ToScalarRuntimeValue(TPrimitive primitive)
    {
        object runtimeValue = primitive;

        if (!_primitive.RuntimeType.IsInstanceOfType(runtimeValue) &&
            Converter.TryConvert(_primitive.RuntimeType, runtimeValue, out var converted))
        {
            runtimeValue = converted;
        }

        return runtimeValue;
    }
}
