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
        while (currentMonth < new DateTime(end.Year, end.Month, 1).AddMonths(1))
        {
            var budget = budgets.SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var overlappingDays = GetOverlappingDays(new Period(start, end), budget);
                totalAmount += budget.GetDailyAmount() * overlappingDays;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
    }

    private static int GetOverlappingDays(Period period, Models.Budget budget)
    {
        var overlappingEnd = budget.LastDay() < period.End ? budget.LastDay() : period.End;
        var overlappingStart = budget.FirstDay() > period.Start ? budget.FirstDay() : period.Start;
        return (overlappingEnd - overlappingStart).Days + 1;
    }
}