using NSubstitute;
using NUnit.Framework.Internal;
using TddBudget.Interface;
using TddBudget.Model;
using TddBudget.Service;

namespace TddBudgetTests;

[TestFixture]
public class BudgetTests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void SetUp()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void get_one_day_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202301",
                Amount = 310
            }
        });

        var actual = WhenQuery(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));
        TotalAmountShouldBe(10m, actual);
    }
    
    [Test]
    public void get_cross_day_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202301",
                Amount = 310
            }
        });

        var actual = WhenQuery(new DateTime(2023, 1, 1), new DateTime(2023, 1, 2));
        TotalAmountShouldBe(20m, actual);
    }

    private static void TotalAmountShouldBe(decimal expected, decimal actual)
    {
        Assert.AreEqual(expected, actual);
    }

    private decimal WhenQuery(DateTime start, DateTime end)
    {
        return _budgetService.Query(start, end);
    }

    private void GivenBudget(List<Budget> budgets)
    {
        _budgetRepo.GetAll().Returns(budgets);
    }
}