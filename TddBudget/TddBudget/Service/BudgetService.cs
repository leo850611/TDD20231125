using TddBudget.Interface;

namespace TddBudget.Service;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        var budgets = _budgetRepo.GetAll();
        if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
        {
            var generateDateRanges = GenerateDateRanges(start, end);
            decimal totalAmount = 0m;
            for (int i = start.Month; i <= end.Month; i++)
            {
                if (i == start.Month)
                {
                    var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
                    var totalDays = (decimal) (new DateTime(2023, start.Month, 31) - start).TotalDays + 1;
                    var oneDayAmount = budget.GetOneDayAmount();
                    totalAmount += totalDays * oneDayAmount;
                }
                else if (i == end.Month)
                {
                    var budget2 = budgets.First(x => x.YearMonth == $"{end.Year}{end.Month:00}");
                    var totalDays2 = (decimal) (end - new DateTime(2023, end.Month, 1)).TotalDays + 1;
                    var oneDayAmount2 = budget2.GetOneDayAmount();
                    totalAmount += totalDays2 * oneDayAmount2;
                }
                else
                {
                    var budget2 = budgets.First(x => x.YearMonth == $"{start.Year}{i:00}");
                    totalAmount += budget2.Amount;
                }
            }

            return totalAmount;
            //
            // decimal totalAmount = 0m;
            // foreach (var dateRange in generateDateRanges)
            // {
            //     var dateRangeItem1 = dateRange.Item1;
            //     var dateRangeItem2 = dateRange.Item2;
            //     var budget = budgets.First(x => x.YearMonth == $"{dateRangeItem1.Year}{dateRangeItem1.Month:00}");
            //     var totalDays = (decimal) (dateRangeItem2 - dateRangeItem1).TotalDays + 1;
            //     var oneDayAmount = budget.GetOneDayAmount();
            //     totalAmount += totalDays * oneDayAmount;
            // }
            //
            // return totalAmount;
            // var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
            // var totalDays = (decimal)(new DateTime(2023, start.Month, 31) - start).TotalDays+1;
            // var oneDayAmount = budget.GetOneDayAmount();
            // var dayAmount1 = totalDays * oneDayAmount;
            //
            // var budget2 = budgets.First(x => x.YearMonth == $"{end.Year}{end.Month:00}");
            // var totalDays2 = (decimal)(new DateTime(2023, end.Month, 1) - end).TotalDays+1;
            // var oneDayAmount2 = budget2.GetOneDayAmount();
            // var dayAmount2 = totalDays2 * oneDayAmount2;
            //
            // return dayAmount1 + dayAmount2;
        }
        else
        {
            var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
            var totalDays = (decimal) (end - start).TotalDays + 1;
            var oneDayAmount = budget.GetOneDayAmount();
            return totalDays * oneDayAmount;
        }
    }

    private static List<Tuple<DateTime, DateTime>> GenerateDateRanges(DateTime start, DateTime end)
    {
        var ranges = new List<Tuple<DateTime, DateTime>>();
        DateTime currentStart = start;

        while (currentStart < end)
        {
            DateTime currentEnd;

            if (currentStart.Month == end.Month && currentStart.Year == end.Year)
            {
                // 特殊處理最後一個月份
                currentEnd = end.AddDays(-1);
            }
            else
            {
                var lastDayOfMonth = new DateTime(currentStart.Year, currentStart.Month,
                    DateTime.DaysInMonth(currentStart.Year, currentStart.Month));
                currentEnd = lastDayOfMonth;
            }

            ranges.Add(new Tuple<DateTime, DateTime>(currentStart, currentEnd));

            // 移至下一個月的第一天
            currentStart = currentEnd.AddDays(1);
        }

        return ranges;
    }
}