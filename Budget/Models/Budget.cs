namespace Budget.Models;

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public int GetDailyAmount()
    {
        var firstDayOfBudget = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return Amount / DateTime.DaysInMonth(firstDayOfBudget.Year, firstDayOfBudget.Month);
    }
}