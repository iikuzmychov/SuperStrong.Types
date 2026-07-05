using SuperStrong.Types.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Querying;

public sealed partial class StrongTypeVisitorTests
{
    [Fact]
    public void Visitor_replaces_AsPrimitive_call_with_cast_to_underlying_type()
    {
        Expression<Func<StrongTypedInt, int>> expression = strongTypedInt => strongTypedInt.AsPrimitive();
        Expression<Func<StrongTypedInt, int>> expectedExpression = strongTypedInt => (int)(object)strongTypedInt;

        var visitedExpression = StrongTypeVisitor.Instance.Visit(expression);

        Assert.Equivalent(expectedExpression, visitedExpression, strict: true);
    }

    [Fact]
    public void Visitor_ignores_AsPrimitive_call_on_non_strong_type()
    {
        Expression<Func<NotAStrongType, int>> expression = notAStrongType => notAStrongType.AsPrimitive();

        var visitedExpression = StrongTypeVisitor.Instance.Visit(expression);

        Assert.Same(expression, visitedExpression);
    }

    private sealed class NotAStrongType
    {
        public int AsPrimitive() => throw new NotImplementedException();
    }

    [StrongType<int>]
    private sealed partial class StrongTypedInt
    {
        public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>();
    }
}
