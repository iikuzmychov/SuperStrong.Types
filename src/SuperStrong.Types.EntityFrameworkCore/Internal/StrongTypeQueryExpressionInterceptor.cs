using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace SuperStrong.Types.EntityFrameworkCore.Internal;

internal sealed class StrongTypeQueryExpressionInterceptor : IQueryExpressionInterceptor
{
    public static StrongTypeQueryExpressionInterceptor Instance { get; } = new();

    private StrongTypeQueryExpressionInterceptor()
    {
    }

    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        return StrongTypeVisitor.Instance.Visit(queryExpression);
    }
}
