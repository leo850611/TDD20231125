using TddBudget.Interface;
using TddBudget.Model;

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
        if (IsInValidQueryPeriod(start, end))
        {
            return 0m;
        }

        var budgets = GetInPeriodBudgets(start, end).ToList();

        if (!budgets.Any())
        {
            return 0m;
        }
        
        if (IsSameYearMonth(start, end))
        {
            var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
            var totalDays = (decimal) (end - start).TotalDays + 1;
            var oneDayAmount = budget.GetOneDayAmount();
            return totalDays * oneDayAmount;
        }
        else
        {
            return 100m;
        }
    }

    private IEnumerable<Budget> GetInPeriodBudgets(DateTime start, DateTime end)
    {
        return _budgetRepo.GetAll().Where(budget => budget.IsInQueryPeriod(start, end));
    }
    
    private static bool IsSameYearMonth(DateTime start, DateTime end)
    {
        return start.ToString("yyyyMM") == end.ToString("yyyyMM");
    }

    private bool IsInValidQueryPeriod(DateTime start, DateTime end)
    {
        return start > end;
    }
}