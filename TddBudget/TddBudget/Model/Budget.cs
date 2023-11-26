namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetAmountByPeriod(Period period)
    {
        var totalDays = (decimal) (period.EndDate - period.StartDate).TotalDays + 1;
        return GetOneDayAmount() * totalDays;
    }

    private decimal GetOneDayAmount()
    {
        var firstDate = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        var daysInMonth = DateTime.DaysInMonth(firstDate.Year, firstDate.Month);
        return Amount / (decimal) daysInMonth;
    }
}