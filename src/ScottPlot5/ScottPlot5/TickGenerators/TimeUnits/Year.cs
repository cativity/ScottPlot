namespace ScottPlot.TickGenerators.TimeUnits;

public class Year : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Years;

    public TimeSpan MinSize => TimeSpan.FromDays(365);

    public DateTime Snap(DateTime dt) => new DateTime(dt.Year, 1, 1);

    public string GetDateTimeFormatString() => "yyyy";

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        // TODO: move this into extension methods file

        int newYear = dateTime.Year + increment;

        return newYear switch
        {
            <= 100 => new DateTime(100, 1, 1),
            > 10_000 => new DateTime(9_999, 1, 1),
            _ => dateTime.AddYears(increment)
        };
    }
}
