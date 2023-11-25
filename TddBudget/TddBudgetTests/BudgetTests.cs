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
    public void get_partial_month_budget()
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

    [Test]
    public void get_one_month_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202301",
                Amount = 310
            }
        });

        var actual = WhenQuery(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        TotalAmountShouldBe(310m, actual);
    }

    [Test]
    public void get_cross_month_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202303",
                Amount = 310
            },
            new()
            {
                YearMonth = "202304",
                Amount = 600
            }
        });

        var actual = WhenQuery(new DateTime(2023, 3, 1), new DateTime(2023, 4, 1));
        TotalAmountShouldBe(330m, actual);
    }

    [Test]
    public void get_cross_two_month_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202303",
                Amount = 310
            },
            new()
            {
                YearMonth = "202304",
                Amount = 600
            },
            new()
            {
                YearMonth = "202305",
                Amount = 3100
            }
        });

        var actual = WhenQuery(new DateTime(2023, 3, 30), new DateTime(2023, 5, 1));
        TotalAmountShouldBe(20m + 600m + 100m, actual);
    }

    [Test]
    public void get_cross_year_budget()
    {
        GivenBudget(new List<Budget>()
        {
            new()
            {
                YearMonth = "202312",
                Amount = 310
            },
            new()
            {
                YearMonth = "202401",
                Amount = 3100
            }
        });

        var actual = WhenQuery(new DateTime(2023, 12, 30), new DateTime(2024, 1, 5));
        TotalAmountShouldBe(20m + 500m, actual);
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