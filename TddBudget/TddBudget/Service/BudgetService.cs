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
        var budgets = _budgetRepo.GetAll();
        var totalAmount = 0m;

        var startDate = start;
        var endDate = end;
        while (startDate <= endDate)
        {
            var lastDateInStartMonth = new DateTime(startDate.Year, startDate.Month,
                DateTime.DaysInMonth(startDate.Year, startDate.Month));
            totalAmount += GetAmount(startDate, lastDateInStartMonth < endDate ? lastDateInStartMonth : end, budgets);

            startDate = lastDateInStartMonth.AddDays(1);
        }

        return totalAmount;
    }

    private decimal GetAmount(DateTime start, DateTime end, IEnumerable<Budget> budgets)
    {
        var budget = budgets.First(x => x.YearMonth == $"{start.Year}{start.Month:00}");
        var totalDays = (decimal) (end - start).TotalDays + 1;
        var oneDayAmount = budget.GetOneDayAmount();
        return totalDays * oneDayAmount;
    }
}