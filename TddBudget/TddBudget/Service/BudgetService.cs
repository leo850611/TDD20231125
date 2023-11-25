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
        if (IsInValidPeriod(start, end))
        {
            return 0m;
        }

        var budgets = GetBudgetsByPeriod(start, end);

        if (!budgets.Any())
        {
            return 0m;
        }
        
        if (IsSameYearMonth(start, end))
        {
            var budget = budgets.Find(x => x.IsSameYearMonth(start));
            return GetDifferenceDays(start, end) * budget!.GetAverageAmountByEveryDay();
        }

        var totalAmount = 0m;
        foreach (var budget in budgets)
        {
            if (budget.IsSameYearMonth(start))
            {
                totalAmount += GetDifferenceDays(start, new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month))) * budget.GetAverageAmountByEveryDay();
            }else if (budget.IsSameYearMonth(end))
            {
                totalAmount +=  GetDifferenceDays(new DateTime(end.Year, end.Month, 1), end) * budget.GetAverageAmountByEveryDay();
            }
            else
            {
               totalAmount += budget.Amount; 
            }
        }

        return totalAmount;
    }

    private decimal GetDifferenceDays(DateTime start, DateTime end)
    {
        return ((decimal) (end - start).TotalDays + 1);
    }

    private bool IsSameYearMonth(DateTime start, DateTime end)
    {
        return start.ToString("yyyyMM") == end.ToString("yyyyMM");
    }

    private List<Budget> GetBudgetsByPeriod(DateTime start, DateTime end)
    {
        return _budgetRepo.GetAll().Where(budget => budget.IsInPeriod(start, end)).ToList();
    }

    private bool IsInValidPeriod(DateTime start, DateTime end)
    {
        return start > end;
    }
}