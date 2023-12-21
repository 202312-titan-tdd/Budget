namespace Budget.Models;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime End { get; private set; }
    public DateTime Start { get; private set; }

    public int GetOverlappingDays(Period another)
    {
        var overlappingEnd = another.End < End ? another.End : End;
        var overlappingStart = another.Start > Start ? another.Start : Start;
        return (overlappingEnd - overlappingStart).Days + 1;
    }
}