namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public sealed class BoolSamples : TheoryData<bool>
{
    public BoolSamples() { Add(true); Add(false); }
}

public sealed class ByteSamples : TheoryData<byte>
{
    public ByteSamples() { Add(0); Add(byte.MaxValue); }
}

public sealed class SByteSamples : TheoryData<sbyte>
{
    public SByteSamples() { Add(sbyte.MinValue); Add(0); Add(sbyte.MaxValue); }
}

public sealed class ShortSamples : TheoryData<short>
{
    public ShortSamples() { Add(short.MinValue); Add(0); Add(short.MaxValue); }
}

public sealed class UShortSamples : TheoryData<ushort>
{
    public UShortSamples() { Add(0); Add(ushort.MaxValue); }
}

public sealed class IntSamples : TheoryData<int>
{
    public IntSamples() { Add(int.MinValue); Add(0); Add(int.MaxValue); }
}

public sealed class UIntSamples : TheoryData<uint>
{
    public UIntSamples() { Add(0); Add(uint.MaxValue); }
}

public sealed class LongSamples : TheoryData<long>
{
    public LongSamples() { Add(long.MinValue); Add(0); Add(long.MaxValue); }
}

public sealed class ULongSamples : TheoryData<ulong>
{
    public ULongSamples() { Add(0); Add(ulong.MaxValue); }
}

public sealed class FloatSamples : TheoryData<float>
{
    public FloatSamples() { Add(float.MinValue); Add(-1.5f); Add(0f); Add(float.MaxValue); }
}

public sealed class DoubleSamples : TheoryData<double>
{
    public DoubleSamples() { Add(double.MinValue); Add(-1.5); Add(0d); Add(double.MaxValue); }
}

public sealed class DecimalSamples : TheoryData<decimal>
{
    // Default decimal(18,2) — values are kept within range/scale.
    public DecimalSamples() { Add(-1.5m); Add(0m); Add(12345.67m); Add(-99999999999999.99m); }
}

public sealed class StringSamples : TheoryData<string>
{
    public StringSamples() { Add("hello"); Add(""); Add(" "); Add("a\"b\\c"); Add("café"); }
}

public sealed class CharSamples : TheoryData<char>
{
    public CharSamples() { Add('a'); Add('Z'); Add(' '); }
}

public sealed class GuidSamples : TheoryData<Guid>
{
    public GuidSamples() { Add(Guid.Empty); Add(Guid.Parse("12345678-1234-1234-1234-1234567890ab")); }
}

public sealed class DateTimeSamples : TheoryData<DateTime>
{
    // Npgsql maps DateTime to timestamptz, which requires UTC.
    public DateTimeSamples()
    {
        Add(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc));
        Add(new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc));
    }
}

public sealed class DateTimeOffsetSamples : TheoryData<DateTimeOffset>
{
    // Npgsql maps DateTimeOffset to timestamptz, which requires offset 0.
    public DateTimeOffsetSamples()
    {
        Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero));
        Add(new DateTimeOffset(2026, 6, 27, 0, 0, 0, TimeSpan.Zero));
    }
}

public sealed class DateOnlySamples : TheoryData<DateOnly>
{
    public DateOnlySamples() { Add(DateOnly.MinValue); Add(new DateOnly(2024, 1, 2)); Add(DateOnly.MaxValue); }
}

public sealed class TimeOnlySamples : TheoryData<TimeOnly>
{
    public TimeOnlySamples() { Add(TimeOnly.MinValue); Add(new TimeOnly(3, 4, 5)); }
}

public sealed class TimeSpanSamples : TheoryData<TimeSpan>
{
    // SQL Server's time column holds 0..24h, so durations are kept non-negative and below a day
    public TimeSpanSamples() { Add(TimeSpan.Zero); Add(new TimeSpan(1, 2, 3)); Add(new TimeSpan(23, 59, 59)); }
}
