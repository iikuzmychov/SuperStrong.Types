using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal interface IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model);

    public void Emit(IndentedWriter writer, StrongTypeModel model);
}
