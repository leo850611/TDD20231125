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
        var year = int.Parse(YearMonth.Substring(0, 4));
        var month = int.Parse(YearMonth.Substring(4, 2));

        var daysInMonth = DateTime.DaysInMonth(year, month);
        return Amount / (decimal) daysInMonth;
    }
}