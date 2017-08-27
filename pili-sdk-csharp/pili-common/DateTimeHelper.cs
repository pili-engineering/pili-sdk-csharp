using System;

internal static class DateTimeHelper
{
#if NET45
    private static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
#else
    private static readonly DateTimeOffset UnixEpoch = DateTimeOffset.FromUnixTimeSeconds(0);
#endif

    internal static long CurrentUnixTimeMillis()
    {
        return (long)(DateTimeOffset.UtcNow - UnixEpoch).TotalMilliseconds;
    }

    internal static long CurrentUnixTimeSeconds()
    {
        return (long)(DateTimeOffset.UtcNow - UnixEpoch).TotalSeconds;
    }
}
