﻿#region

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
        var totalAmount = GetBudgetPeriod(start, end, budgets);
        return totalAmount;
        // var budgetPeriod = GetBudgetPeriod(start, end, budgets);

        // return budgets
        //        .Join(
        //            budgetPeriod,
        //            budget => budget.YearMonth,
        //            period => period.Key.ToString("yyyyMM"),
        //            (budget, period) =>
        //            {
        //                var singleDayAmount = budget.Amount / DateTime.DaysInMonth(period.Key.Year, period.Key.Month);
        //                return singleDayAmount * period.Value;
        //            })
        //        .Sum(monthAmount => monthAmount);
    }

    // private Dictionary<DateTime, int> GetBudgetPeriod(DateTime startDate, DateTime endDate, List<Models.Budget> budgets)
    private decimal GetBudgetPeriod(DateTime startDate, DateTime endDate, List<Models.Budget> budgets)
    {
        var period = new Dictionary<DateTime, int>();
        var currentMonth = startDate;

        // return budgets
        //        .Join(
        //            budgetPeriod,
        //            budget => budget.YearMonth,
        //            period => period.Key.ToString("yyyyMM"),
        //            (budget, period) =>
        //            {
        //                var singleDayAmount = budget.Amount / DateTime.DaysInMonth(period.Key.Year, period.Key.Month);
        //                return singleDayAmount * period.Value;
        //            })
        //        .Sum(monthAmount => monthAmount);
        var totalAmount = 0m;
        while (currentMonth <= endDate)
        {
            var endOfMonth = new DateTime(
                currentMonth.Year, currentMonth.Month,
                DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month));
            var endOfPeriod = (endOfMonth < endDate) ? endOfMonth : endDate;
            var daysInMonth = (endOfPeriod - currentMonth).Days + 1;
            period.Add(currentMonth, daysInMonth);
            var budget = budgets.SingleOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
            if (budget != null)
            {
                var singleDayAmount = budget.Amount / DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                totalAmount += singleDayAmount * daysInMonth;
            }

            var nextMonth = currentMonth.AddMonths(1);
            currentMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }

        return totalAmount;
        // return period;
    }
}