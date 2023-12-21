#region

using Budget.Repositories;

#endregion

namespace Budget.Services;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime End { get; private set; }
    public DateTime Start { get; private set; }

    public int GetOverlappingDays(Period another)
    {
        var overlappingEnd = another.End < End ? another.End : End;
        var overlappingStart = another.Start > Start ? another.Start : Start;
        return (overlappingEnd - overlappingStart).Days + 1;
    }
}

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

        var budgets = budgetRepository.GetAll();
        var period = new Period(start, end);

        return budgets.Sum(budget => budget.GetOverlappingAmount(period));
    }
}