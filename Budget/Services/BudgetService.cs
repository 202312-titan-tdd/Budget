#region

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

        var currentMonth = start;

        var totalAmount = 0m;
        var budgets = budgetRepository.GetAll();
        while (currentMonth < new DateTime(end.Year, end.Month, 1).AddMonths(1))
        {
            var budget = budgets.SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var overlappingDays = GetOverlappingDays(start, end, budget);
                totalAmount += budget.GetDailyAmount() * overlappingDays;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
    }

    private static int GetOverlappingDays(DateTime start, DateTime end, Models.Budget budget)
    {
        var endOfPeriod = budget.LastDay() < end ? budget.LastDay() : end;
        var startOfPeriod = budget.FirstDay() > start ? budget.FirstDay() : start;
        var overlappingDays = (endOfPeriod - startOfPeriod).Days + 1;
        return overlappingDays;
    }
}