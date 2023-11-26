using System.Globalization;

namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetAmount(DateTime start, DateTime end)
    {
        var firstDayOfMonth = GetFirstDayOfMonth();
        var lastDayOfMonth = GetLastDayOfMonth();

        if (start >= firstDayOfMonth && end <= lastDayOfMonth)
        {
            var differenceDays = GetDifferenceDays(start, end);
            return differenceDays * (Amount / DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month));
        }
        else if (start <= firstDayOfMonth && lastDayOfMonth <= end)
        {
            return Amount;
        }
        else if (start <= firstDayOfMonth && firstDayOfMonth <= end)
        {
            var differenceDays = GetDifferenceDays(new DateTime(end.Year, end.Month, 1), end);
            return differenceDays * (Amount / DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month));
        }
        else if (start <= lastDayOfMonth && lastDayOfMonth <= end)
        {
            var differenceDays = GetDifferenceDays(start,
                new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month)));
            return differenceDays * (Amount / DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month));
        }

        return 0m;
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

    private decimal GetDifferenceDays(DateTime start, DateTime end)
    {
        return ((decimal)(end - start).TotalDays + 1);
    }
}