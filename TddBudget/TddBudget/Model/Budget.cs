using System.Globalization;

namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetAverageAmountByEveryDay()
    {
        var yearMonth = GetFirstDayOfMonth();
        return Amount / DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month);
    }

    public bool IsSameYearMonth(DateTime start)
    {
        return YearMonth == $"{start.Year}{start.Month:00}";
    }

    public bool IsInPeriod(DateTime start, DateTime end)
    {
        var firstDayOfMonth = GetFirstDayOfMonth();
        var lastDayOfMonth = GetLastDayOfMonth();

        return (firstDayOfMonth >= start && firstDayOfMonth <= end)
               || (lastDayOfMonth >= start && lastDayOfMonth <= end)
               || (firstDayOfMonth <= start && lastDayOfMonth >= end);
    }

    private DateTime GetLastDayOfMonth()
    {
        return GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
    }

    private DateTime GetFirstDayOfMonth()
    {
        var yearMonth = GetYearMonth();
        return new DateTime(yearMonth.Year, yearMonth.Month, 1);
    }

    private DateTime GetYearMonth()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
    }
}