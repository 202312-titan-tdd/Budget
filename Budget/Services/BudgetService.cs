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

    public int GetOverlappingDays(Models.Budget budget)
    {
        var overlappingEnd = budget.LastDay() < End ? budget.LastDay() : End;
        var overlappingStart = budget.FirstDay() > Start ? budget.FirstDay() : Start;
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

        var currentMonth = start;

        var totalAmount = 0m;
        var budgets = budgetRepository.GetAll();
        var period = new Period(start, end);
        while (currentMonth < new DateTime(end.Year, end.Month, 1).AddMonths(1))
        {
            var budget = budgets.SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var overlappingDays = period.GetOverlappingDays(budget);
                totalAmount += budget.GetDailyAmount() * overlappingDays;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
    }
}