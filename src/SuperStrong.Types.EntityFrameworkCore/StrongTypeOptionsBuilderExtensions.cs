using System.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore;

public static class StrongTypeOptionsBuilderExtensions
{
    public static StrongTypeOptionsBuilder AddValidatorAdaptersFromAssembly<TAssemblyMarker>(
        this StrongTypeOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.AddValidatorAdaptersFromAssembly(typeof(TAssemblyMarker).Assembly, _ => true);
    }

    public static StrongTypeOptionsBuilder AddValidatorAdaptersFromAssembly<TAssemblyMarker>(
        this StrongTypeOptionsBuilder builder,
        Func<Type, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.AddValidatorAdaptersFromAssembly(typeof(TAssemblyMarker).Assembly, filter);
    }

    public static StrongTypeOptionsBuilder AddValidatorAdaptersFromAssembly(
        this StrongTypeOptionsBuilder builder,
        Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assembly);

        return builder.AddValidatorAdaptersFromAssembly(assembly, _ => true);
    }

    public static StrongTypeOptionsBuilder AddValidatorAdaptersFromAssembly(
        this StrongTypeOptionsBuilder builder,
        Assembly assembly,
        Func<Type, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(filter);

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
            {
                continue;
            }

            if (!typeof(StrongTypeValidatorAdapter).IsAssignableFrom(type))
            {
                continue;
            }

            if (type.GetConstructor(Type.EmptyTypes) is null)
            {
                continue;
            }

            if (!filter(type))
            {
                continue;
            }

            var adapter = (StrongTypeValidatorAdapter)Activator.CreateInstance(type)!;
            builder.AddValidatorAdapter(adapter);
        }

        return builder;
    }
}
