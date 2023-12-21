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

        var budgets = budgetRepository.GetAll();
        return GetTotalAmount(start, end, budgets);
    }

    private static int GetDailyAmount(Models.Budget budget)
    {
        var firstDayOfBudget = DateTime.ParseExact(budget.YearMonth, "yyyyMM", null);
        return budget.Amount / DateTime.DaysInMonth(firstDayOfBudget.Year, firstDayOfBudget.Month);
    }

    private decimal GetTotalAmount(DateTime startDate, DateTime endDate, List<Models.Budget> budgets)
    {
        var currentMonth = startDate;

        var totalAmount = 0m;
        while (currentMonth <= endDate)
        {
            var budget = budgets.SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var endOfMonth = new DateTime(
                    currentMonth.Year, currentMonth.Month,
                    DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month));
                var endOfPeriod = (endOfMonth < endDate) ? endOfMonth : endDate;
                var daysInMonth = (endOfPeriod - currentMonth).Days + 1;
                var dailyAmount = GetDailyAmount(budget);
                totalAmount += dailyAmount * daysInMonth;
            }

            var nextMonth = currentMonth.AddMonths(1);
            currentMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }

        return totalAmount;
    }
}