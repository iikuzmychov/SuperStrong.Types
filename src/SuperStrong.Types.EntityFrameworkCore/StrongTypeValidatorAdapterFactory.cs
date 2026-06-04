using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore;

public abstract class StrongTypeValidatorAdapterFactory : StrongTypeValidatorAdapter
{
    public abstract StrongTypeValidatorAdapter CreateValidatorAdapter(Type validatorType);

    internal sealed override void Apply(
        Type validatorType,
        IReadOnlyList<StrongTypeValidator> validators,
        IConventionProperty property,
        Type strongType)
    {
        if (validators.Count == 0)
        {
            return;
        }

        var validatorAdapter = CreateValidatorAdapter(validatorType);
        validatorAdapter.Apply(validatorType, validators, property, strongType);
    }
}
