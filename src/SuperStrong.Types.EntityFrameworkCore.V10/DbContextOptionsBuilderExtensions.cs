using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Internal;

namespace SuperStrong.Types.EntityFrameworkCore;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseStrongTypes<TContext>(
        this DbContextOptionsBuilder<TContext> builder)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);

        ((DbContextOptionsBuilder)builder).UseStrongTypes();

        return builder;
    }

    public static DbContextOptionsBuilder UseStrongTypes(this DbContextOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddInterceptors(StrongTypeQueryExpressionInterceptor.Instance);

        var extension = builder.Options.FindExtension<StrongTypeDbContextOptionsExtension>() ?? new();
        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);

        return builder;
    }
}
