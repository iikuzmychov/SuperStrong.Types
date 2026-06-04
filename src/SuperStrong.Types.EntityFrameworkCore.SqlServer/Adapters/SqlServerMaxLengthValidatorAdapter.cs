using Microsoft.EntityFrameworkCore.Metadata;
using SuperStrong.Types.EntityFrameworkCore.Adapters;
using SuperStrong.Types.Validators;

namespace SuperStrong.Types.EntityFrameworkCore.SqlServer.Adapters;

public sealed class SqlServerMaxLengthValidatorAdapter : StrongTypeValidatorAdapter<MaxLengthValidator, string>
{
    protected override void ApplyCore<TStrongType>(
        IReadOnlyList<MaxLengthValidator> validators,
        IConventionProperty property)
    {
        var maxLength = validators.Min(validator => validator.MaxLength);

        property.Builder.HasMaxLength(maxLength);
    }
}
