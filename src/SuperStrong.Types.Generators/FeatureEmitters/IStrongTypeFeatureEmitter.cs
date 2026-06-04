using SuperStrong.Types.Generators.Helpers;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal interface IStrongTypeFeatureEmitter
{
    public void Emit(IndentedWriter writer, StrongTypeModel model);
}
