using Budget.Services;

namespace Budget.Models;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public DateTime FirstDay()
    {
        var firstDayOfBudget = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return firstDayOfBudget;
    }

    public int GetDailyAmount()
    {
        return Amount / DaysInMonth();
    }

    private int DaysInMonth()
    {
        return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
    }

    public DateTime LastDay()
    {
        return DateTime.ParseExact(YearMonth + DaysInMonth(), "yyyyMMdd", null);
    }

    public Period CreatePeriod()
    {
        return new Period(FirstDay(), LastDay());
    }
}