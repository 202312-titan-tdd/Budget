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
        var firstDayOfBudget = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return Amount / DateTime.DaysInMonth(firstDayOfBudget.Year, firstDayOfBudget.Month);
    }
}