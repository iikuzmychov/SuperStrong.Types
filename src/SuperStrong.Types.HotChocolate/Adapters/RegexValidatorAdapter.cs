using SuperStrong.Types.HotChocolate.Directives;
using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace SuperStrong.Types.HotChocolate.Adapters;

public sealed class RegexValidatorAdapter : StrongTypeValidatorAdapter<RegexValidator, string, RegexDirective>
{
    protected override ImmutableArray<RegexDirective> CreateDirectivesCore(
        IReadOnlyList<RegexValidator> validators)
    {
        return validators
            .Select(validator => new RegexDirective
            {
                Pattern = validator.Regex.ToString(),
                Options = validator.Regex.Options == RegexOptions.None ? null : validator.Regex.Options.ToString(),
            })
            .ToImmutableArray();
    }
}
