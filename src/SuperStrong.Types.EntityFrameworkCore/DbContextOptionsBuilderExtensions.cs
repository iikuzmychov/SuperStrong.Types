using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Internal;

namespace SuperStrong.Types.EntityFrameworkCore;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseStrongTypes<TContext>(
        this DbContextOptionsBuilder<TContext> builder,
        Action<StrongTypeOptionsBuilder>? configure = null)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);

        ((DbContextOptionsBuilder)builder).UseStrongTypes(configure);

        return builder;
    }

    public static DbContextOptionsBuilder UseStrongTypes(
        this DbContextOptionsBuilder builder,
        Action<StrongTypeOptionsBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddInterceptors(StrongTypeQueryExpressionInterceptor.Instance);

        var extension = builder.Options.FindExtension<StrongTypeDbContextOptionsExtension>() ?? new();
        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);

        configure?.Invoke(new StrongTypeOptionsBuilder(extension));

        return builder;
    }
}
