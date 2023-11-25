using System.Globalization;

namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetAverageAmountByEveryDay()
    {
        var yearMonth = GetYearMonth();
        return Amount / DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month);;
    }
    
    public bool IsInPeriod(DateTime start, DateTime end)
    {
        var yearMonth = GetYearMonth();
        return yearMonth >= start && yearMonth <= end;
    }

    private DateTime GetYearMonth()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
    }

    public bool IsSameYearMonth(DateTime start)
    {
        return YearMonth == $"{start.Year}{start.Month:00}";
    }
}