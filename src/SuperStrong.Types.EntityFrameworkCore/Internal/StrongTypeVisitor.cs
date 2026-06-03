using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

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
        if (node.Object is not null &&
            !node.Method.IsGenericMethod &&
            node.Method.Name is nameof(IStrongType<,>.AsPrimitive) &&
            node.Method.GetParameters().Length == 0)
        {
            var primitiveType = node.Method.ReturnType;
            var strongTypeInterface = typeof(IStrongType<,>).MakeGenericType(node.Object.Type, primitiveType);

            if (!node.Object.Type.GetInterfaces().Contains(strongTypeInterface) ||
                !node.Object.Type.GetInterfaceMap(strongTypeInterface).TargetMethods.Contains(node.Method))
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

        replacement = null;
        return false;
    }
}
