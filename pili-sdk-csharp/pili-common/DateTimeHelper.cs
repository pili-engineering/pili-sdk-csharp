using System;

internal static class DateTimeHelper
{
    private static readonly DateTimeOffset UnixEpoch = DateTimeOffset.FromUnixTimeSeconds(0);

    internal static long CurrentUnixTimeMillis()
    {
        return (long)(DateTimeOffset.UtcNow - UnixEpoch).TotalMilliseconds;
    }
}
