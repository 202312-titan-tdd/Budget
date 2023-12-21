#region

using Budget.Services;

#endregion

namespace Budget.Models;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; } = null!;

    public decimal GetOverlappingAmount(Period period)
    {
        return GetDailyAmount() * period.GetOverlappingDays(CreatePeriod());
    }

    private Period CreatePeriod()
    {
        return new Period(FirstDay(), LastDay());
    }

    private int DaysInMonth()
    {
        return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
    }

    private DateTime FirstDay()
    {
        var firstDayOfBudget = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return firstDayOfBudget;
    }

    private decimal GetDailyAmount()
    {
        return (decimal)Amount / DaysInMonth();
    }

    private DateTime LastDay()
    {
        return DateTime.ParseExact(YearMonth + DaysInMonth(), "yyyyMMdd", null);
    }
}