using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

public sealed class NpgsqlMaxLengthValidatorAdapter : StrongTypeValidatorAdapter<MaxLengthValidator, string>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MaxLengthValidator> validators,
        IConventionProperty property)
    {
        var maxLength = validators.Min(validator => validator.MaxLength);

        property.Builder.HasMaxLength(maxLength);
    }
}
