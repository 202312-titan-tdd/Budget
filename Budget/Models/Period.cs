namespace Budget.Models;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    private DateTime End { get; set; }
    private DateTime Start { get; set; }

    public int GetOverlappingDays(Period another)
    {
        var overlappingEnd = another.End < End ? another.End : End;
        var overlappingStart = another.Start > Start ? another.Start : Start;
        return (overlappingEnd - overlappingStart).Days + 1;
    }
}