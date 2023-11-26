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
        return IsInValidPeriod(start, end) ? 0m : _budgetRepo.GetAll().Sum(b => b.GetAmount(start, end));
    }
    
    private bool IsInValidPeriod(DateTime start, DateTime end)
    {
        return start > end;
    }
}