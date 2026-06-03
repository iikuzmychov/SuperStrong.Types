using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.Adapters;

public sealed class MaxLengthValidatorAdapter : StrongTypeValidatorAdapter<MaxLengthValidator, string>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MaxLengthValidator> validators,
        IConventionProperty property)
    {
        var maxLength = validators.Min(validator => validator.MaxLength);

        property.Builder.HasMaxLength(maxLength);
    }
}
