using System;

namespace LightningTalk.Utils
{
    internal static class DateTimeExtensions
    {
        internal static long GetTotalSecondsSinceEpoch(this DateTime dateTime)
        {
            return (long) dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
