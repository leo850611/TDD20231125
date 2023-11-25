namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetOneDayAmount()
    {
        return Amount / DateTime.DaysInMonth(GetYear(), GetMonth());
    }

    public bool IsInQueryPeriod(DateTime start, DateTime end)
    {
        if (DateTime.TryParseExact(YearMonth, "yyyyMM",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out var yearMonth))
        {
            return yearMonth >= start && yearMonth <= end;
        }

        return false;
    }

    private int GetMonth()
    {
        return int.Parse(YearMonth.Substring(4, 2));
    }

    private int GetYear()
    {
        return int.Parse(YearMonth.Substring(0, 4));
    }
}