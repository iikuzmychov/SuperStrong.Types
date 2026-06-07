using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeVisitor : ExpressionVisitor
{
    public static StrongTypeVisitor Instance { get; } = new();

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        // strongType.AsPrimitive() --> (primitive)(object)strongType
        if (TryReplaceAsPrimitiveCall(node, out var asPrimitiveReplacement))
        {
            return Visit(asPrimitiveReplacement);
        }

        return base.VisitMethodCall(node);
    }

    private static bool TryReplaceAsPrimitiveCall(
        MethodCallExpression node,
        [MaybeNullWhen(false)] out Expression replacement)
    {
        if (node.Object is null ||
            node.Method.IsGenericMethod ||
            node.Method.Name is not nameof(IStrongType<,>.AsPrimitive) ||
            node.Method.GetParameters().Length != 0)
        {
            replacement = null;
            return false;
        }

        var primitiveType = node.Method.ReturnType;
        var strongTypeInterface = typeof(IStrongType<,>).MakeGenericType(node.Object.Type, primitiveType);

        if (!node.Object.Type.GetInterfaces().Contains(strongTypeInterface))
        {
            replacement = null;
            return false;
        }

        var interfaceMap = node.Object.Type.GetInterfaceMap(strongTypeInterface);

        var asPrimitiveMethod = node.Object.Type.GetMethod(
            nameof(IStrongType<,>.AsPrimitive),
            bindingAttr: BindingFlags.Public | BindingFlags.Instance,
            types: Type.EmptyTypes);

        if (!interfaceMap.TargetMethods.Contains(node.Method) && node.Method != asPrimitiveMethod)
        {
            replacement = null;
            return false;
        }

        replacement = Expression.Convert(
            expression: Expression.Convert(
                expression: node.Object,
                type: typeof(object)),
            type: primitiveType);

        return true;
    }
}
