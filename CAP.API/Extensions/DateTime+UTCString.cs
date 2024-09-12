using System;

namespace CAP.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static string DBTimeToUTCString(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToString("o", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static DateTime DBTimeToUTCTime(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
