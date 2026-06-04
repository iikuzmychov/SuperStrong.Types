using System.Reflection;

namespace SuperStrong.Types.EntityFrameworkCore;

public static class StrongTypeOptionsBuilderExtensions
{
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
        Func<Type, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (var type in assembly.DefinedTypes)
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
            {
                continue;
            }

            if (!typeof(StrongTypeValidatorAdapter).IsAssignableFrom(type))
            {
                continue;
            }

            var parameterlessConstructor = type.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                Type.EmptyTypes);

            if (parameterlessConstructor is null)
            {
                continue;
            }

            if (!predicate(type))
            {
                continue;
            }

            var adapter = (StrongTypeValidatorAdapter)Activator.CreateInstance(type, nonPublic: true)!;
            builder.AddValidatorAdapter(adapter);
        }

        return builder;
    }
}
