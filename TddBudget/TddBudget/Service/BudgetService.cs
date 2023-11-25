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
        var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
        var totalDays = (decimal)(end - start).TotalDays+1;
        var oneDayAmount = budget.GetOneDayAmount();
        return totalDays * oneDayAmount;
    }
}