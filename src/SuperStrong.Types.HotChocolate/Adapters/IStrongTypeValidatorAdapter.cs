using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Adapters;

public interface IStrongTypeValidatorAdapter
{
    ImmutableArray<object> CreateDirectives(IReadOnlyList<object> validators);
}
