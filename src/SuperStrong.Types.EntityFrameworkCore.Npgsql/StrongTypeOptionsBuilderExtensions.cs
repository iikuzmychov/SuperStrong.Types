namespace SuperStrong.Types.EntityFrameworkCore.Npgsql;

public static class StrongTypeOptionsBuilderExtensions
{
    public static StrongTypeOptionsBuilder AddNpgsqlValidatorAdapters(this StrongTypeOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder
            .AddDefaultValidatorAdapters()
            .AddValidatorAdaptersFromAssembly<ThisAssemblyMarker>();
    }
}
