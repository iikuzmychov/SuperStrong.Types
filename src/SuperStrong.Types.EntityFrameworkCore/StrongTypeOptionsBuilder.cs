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
}
