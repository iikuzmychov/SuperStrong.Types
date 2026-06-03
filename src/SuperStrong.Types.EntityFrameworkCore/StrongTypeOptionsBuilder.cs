using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.EntityFrameworkCore.Internal;

namespace SuperStrong.Types.EntityFrameworkCore;

public sealed class StrongTypeOptionsBuilder
{
    private readonly StrongTypeDbContextOptionsExtension _extension;

    internal StrongTypeOptionsBuilder(StrongTypeDbContextOptionsExtension extension)
    {
        _extension = extension;
    }

    public StrongTypeOptionsBuilder AddValidatorAdapter(StrongTypeValidatorAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);

        _extension.RegisterAdapter(adapter);

        return this;
    }

    public StrongTypeOptionsBuilder AddValidatorAdapter(Type adapterType)
    {
        ArgumentNullException.ThrowIfNull(adapterType);

        if (!typeof(StrongTypeValidatorAdapter).IsAssignableFrom(adapterType))
        {
            throw new ArgumentException(
                $"{adapterType} must derive from {nameof(StrongTypeValidatorAdapter)}.",
                nameof(adapterType));
        }

        if (adapterType.IsGenericTypeDefinition)
        {
            throw new ArgumentException(
                $"{adapterType} is an open generic type; register a {nameof(StrongTypeValidatorAdapterFactory)} subclass instead.",
                nameof(adapterType));
        }

        if (adapterType.GetConstructor(Type.EmptyTypes) is null)
        {
            throw new ArgumentException(
                $"{adapterType} must have a public parameterless constructor.",
                nameof(adapterType));
        }

        var adapter = (StrongTypeValidatorAdapter)Activator.CreateInstance(adapterType)!;

        _extension.RegisterAdapter(adapter);

        return this;
    }
}
