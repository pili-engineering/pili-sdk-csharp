using System;

internal static class DateTimeHelperClass
{
    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    internal static long CurrentUnixTimeMillis()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }
}
