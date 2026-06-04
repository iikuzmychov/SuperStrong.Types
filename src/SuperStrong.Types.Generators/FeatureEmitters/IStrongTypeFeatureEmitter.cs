using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal interface IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model);

    public void Emit(IndentedWriter writer, StrongTypeModel model);
}
