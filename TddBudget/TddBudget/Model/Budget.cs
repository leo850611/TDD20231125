namespace TddBudget.Model;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public decimal GetAmountByPeriod(Period period)
    {
        var start = period.StartDate;
        var end = period.EndDate;

        var totalDays = (decimal) (end - start).TotalDays + 1;
        var oneDayAmount = GetOneDayAmount();
        return totalDays * oneDayAmount;
    }

    private decimal GetOneDayAmount()
    {
        var year = int.Parse(YearMonth.Substring(0, 4));
        var month = int.Parse(YearMonth.Substring(4, 2));

        var daysInMonth = DateTime.DaysInMonth(year, month);
        return Amount / daysInMonth;
    }
}