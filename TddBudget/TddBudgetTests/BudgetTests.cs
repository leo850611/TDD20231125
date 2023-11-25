using NSubstitute;
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

    [Test]
    public void Query_Period_No_Any_Budget()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 600,
                YearMonth = "202302"
            }
        });

        var actual = WhenQuery(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1));

        TotalAmountShouldBe(0m, actual);
    }

    [Test]
    public void Query_Period_With_One_Day()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 600,
                YearMonth = "202304"
            }
        });

        var actual = WhenQuery(new DateTime(2023, 4, 10), new DateTime(2023, 4, 10));

        TotalAmountShouldBe(20m, actual);
    }

    [Test]
    public void Query_Period_With_Partial_Day_Of_Month()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 600,
                YearMonth = "202304"
            }
        });

        var actual = WhenQuery(new DateTime(2023, 4, 1), new DateTime(2023, 4, 15));

        TotalAmountShouldBe(300m, actual);
    }

    [Test]
    public void Query_Period_With_Cross_Month()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 620,
                YearMonth = "202303"
            },
            new()
            {
                Amount = 900,
                YearMonth = "202304"
            },
            new()
            {
                Amount = 930,
                YearMonth = "202305"
            },
        });

        var actual = WhenQuery(new DateTime(2023, 1, 1), new DateTime(2023, 5, 15));

        TotalAmountShouldBe(1970m, actual);
    }

    [Test]
    public void Query_Period_With_Cross_Year()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 620,
                YearMonth = "202103"
            },
            new()
            {
                Amount = 900,
                YearMonth = "202304"
            },
            new()
            {
                Amount = 930,
                YearMonth = "202305"
            },
        });

        var actual = WhenQuery(new DateTime(2020, 1, 1), new DateTime(2023, 5, 15));

        TotalAmountShouldBe(1970m, actual);
    }

    [Test]
    public void Query_Period_With_Leap_Year()
    {
        GivenAllBudget(new List<Budget>()
        {
            new()
            {
                Amount = 580,
                YearMonth = "201602"
            },
            new()
            {
                Amount = 900,
                YearMonth = "202304"
            },
            new()
            {
                Amount = 930,
                YearMonth = "202305"
            },
        });

        var actual = WhenQuery(new DateTime(2016, 2, 28), new DateTime(2023, 5, 15));

        TotalAmountShouldBe(1390m, actual);
    }


    private void GivenAllBudget(List<Budget> budgets)
    {
        _budgetRepo.GetAll().Returns(budgets);
    }

    private void TotalAmountShouldBe(decimal expected, decimal actual)
    {
        Assert.That(actual, Is.EqualTo(expected));
    }

    private decimal WhenQuery(DateTime start, DateTime end)
    {
        return _budgetService.Query(start, end);
    }
}