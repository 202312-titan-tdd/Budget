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

        return GetTotalAmount(start, end);
    }

    private decimal GetTotalAmount(DateTime startDate, DateTime endDate)
    {
        var currentMonth = startDate;

        var totalAmount = 0m;
        while (currentMonth < new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1))
        {
            var budget = budgetRepository.GetAll().SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var endOfPeriod = budget.LastDay() < endDate ? budget.LastDay() : endDate;
                var startOfPeriod = budget.FirstDay() > startDate ? budget.FirstDay() : startDate;
                var daysInMonth = (endOfPeriod - startOfPeriod).Days + 1;
                totalAmount += budget.GetDailyAmount() * daysInMonth;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
    }
}