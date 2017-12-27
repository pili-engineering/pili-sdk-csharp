using System;

namespace Qiniu.Pili.Internal
{
#if NET45
    internal static class DateTimeOffsetExtensions
    {
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTime)
        {
            return dateTime.UtcDateTime.Ticks / 10000000L - 62135596800L;
        }

        public static long ToUnixTimeMilliseconds(this DateTimeOffset dateTime)
        {
            return dateTime.UtcDateTime.Ticks / 10000L - 62135596800000L;
        }
    }
#endif
}
