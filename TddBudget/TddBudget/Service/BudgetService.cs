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
            var totalAmount = 0m;
            for (var i = start.Month; i <= end.Month; i++)
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
        }
        else
        {
            var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
            var totalDays = (decimal) (end - start).TotalDays + 1;
            var oneDayAmount = budget.GetOneDayAmount();
            return totalDays * oneDayAmount;
        }
    }
}