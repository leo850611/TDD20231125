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
    public void Query_Period_Is_InValid()
    { 
        var actual = WhenQuery(new DateTime(2023, 1, 10), new DateTime(2023, 1, 1));
        TotalAmountShouldBe(0m, actual); 
    }

    
    private void TotalAmountShouldBe(decimal expected, decimal actual)
    {
        Assert.That(actual, Is.EqualTo(expected));
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