namespace Educational.Core.BLL.Helpers;

public static class DateTimeHelper
{
    public static long ToUnixEpochDate(DateTime date)
    {
        return new DateTimeOffset(date.ToUniversalTime()).ToUnixTimeSeconds();
    }

    public static DateTime FromUnixEpochDate(long date)
    {
        return DateTimeOffset.FromUnixTimeSeconds(date).UtcDateTime;
    }
}
