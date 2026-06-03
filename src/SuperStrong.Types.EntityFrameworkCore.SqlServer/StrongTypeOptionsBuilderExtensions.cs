namespace SuperStrong.Types.EntityFrameworkCore.SqlServer;

public static class StrongTypeOptionsBuilderExtensions
{
    public static StrongTypeOptionsBuilder AddSqlServerValidatorAdapters(this StrongTypeOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder
            .AddDefaultValidatorAdapters()
            .AddValidatorAdaptersFromAssembly<ThisAssemblyMarker>();
    }
}
