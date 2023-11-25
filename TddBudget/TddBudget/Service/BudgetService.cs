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

        var budgets = GetInPeriodBudgets(start, end);

        if (!budgets.Any())
        {
            return 0m;
        }
        
        if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
        {
            var budget = budgets.Find(x => x.IsSameYearMonth(start));
            return ((decimal) (end - start).TotalDays + 1) * budget!.GetAverageAmountByEveryDay();
        }

        var totalAmount = 0m;
        foreach (var budget in budgets)
        {
            if (budget.IsSameYearMonth(start))
            {
                totalAmount += ((decimal)(new DateTime(start.Year, start.Month, 31) - start).TotalDays + 1) * budget.GetAverageAmountByEveryDay();
            }else if (budget.IsSameYearMonth(end))
            {
                totalAmount += ((decimal)(end - new DateTime(end.Year, end.Month, 1)).TotalDays +
                                1) * budget.GetAverageAmountByEveryDay();
            }
            else
            {
               totalAmount += budget.Amount; 
            }
        }

        return totalAmount;
    }

    private List<Budget> GetInPeriodBudgets(DateTime start, DateTime end)
    {
        return _budgetRepo.GetAll().Where(budget => budget.IsInPeriod(start, end)).ToList();
    }

    private bool IsInValidQueryPeriod(DateTime start, DateTime end)
    {
        return start > end;
    }
}