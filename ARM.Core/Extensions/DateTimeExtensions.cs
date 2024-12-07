namespace ARM.Core.Extensions;

public static class DateTimeExtensions
{

    /// <summary>
    /// Получить дату с первым днём месяца
    /// </summary>
    public static DateTime WithFirstDayMonth(this DateTime date)
        => date.AddDays(-date.Day + 1);
    
    /// <summary>
    /// Получить дату с последним днём месяца
    /// </summary>
    public static DateTime WithLastDayMonth(this DateTime date)
        => date.WithFirstDayMonth().AddMonths(1).AddDays(-1);
    
}