using TddBudget.Model;

namespace TddBudget.Interface;

public interface IBudgetRepo
{
    List<Budget> GetAll();
}