namespace ScottPlot.TickGenerators.TimeUnits;

public class Day : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Days;

    public TimeSpan MinSize => TimeSpan.FromDays(1);

    public DateTime Snap(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day);

    public string GetDateTimeFormatString() => "d";

    public DateTime Next(DateTime dateTime, int increment = 1) => dateTime.AddDays(increment);
}
