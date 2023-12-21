#region

using Budget.Models;
using Budget.Repositories;

#endregion

namespace Budget.Services;

public class BudgetService
{
    private readonly IBudgetRepository budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        this.budgetRepository = budgetRepository;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0;
        }

        var period = new Period(start, end);

        return budgetRepository.GetAll()
                               .Sum(budget => budget.GetOverlappingAmount(period));
    }
}