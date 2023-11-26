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

        var current = start;
        while (current <= end)
        {
            // TODO : currentLastDateInMonth can use budget last day
            var currentLastDateInMonth = GetLastDateOfMonth(current);
            var period = new Period
            {
                StartDate = current,
                EndDate = currentLastDateInMonth < end ? currentLastDateInMonth : end
            };

            var budget = budgets.FirstOrDefault(x => x.YearMonth == current.ToString("yyyyMM"));
            totalAmount += budget?.GetAmountByPeriod(period) ?? 0;
            current = currentLastDateInMonth.AddDays(1);
        }

        return totalAmount;
    }

    private static DateTime GetLastDateOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month,
            DateTime.DaysInMonth(date.Year, date.Month));
    }
}