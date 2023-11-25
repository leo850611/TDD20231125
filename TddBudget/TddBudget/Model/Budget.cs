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

    public DateTime GetLastDayOfMonth()
    {
        return GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
    }

    public DateTime GetFirstDayOfMonth()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
    }

    public bool IsInPeriod(DateTime start, DateTime end)
    {
        var firstDayOfMonth = GetFirstDayOfMonth();
        var lastDayOfMonth = GetLastDayOfMonth();

        return (firstDayOfMonth >= start && firstDayOfMonth <= end)
               || (lastDayOfMonth >= start && lastDayOfMonth <= end)
               || (firstDayOfMonth <= start && lastDayOfMonth >= end);
    }
}