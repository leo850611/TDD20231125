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
        while (startDate <= end)
        {
            var lastDateInStartMonth = GetLastDateByDate(startDate);
            var period = new Period
            {
                StartDate = startDate,
                EndDate = lastDateInStartMonth < end ? lastDateInStartMonth : end
            };

            var budget = budgets.First(x => x.YearMonth == $"{startDate.Year}{startDate.Month:00}");
            totalAmount += budget.GetAmountByPeriod(period);
            startDate = lastDateInStartMonth.AddDays(1);
        }

        return totalAmount;
    }

    private static DateTime GetLastDateByDate(DateTime startDate)
    {
        return new DateTime(startDate.Year, startDate.Month,
            DateTime.DaysInMonth(startDate.Year, startDate.Month));
    }
}