namespace ScottPlot.TickGenerators.TimeUnits;

public class Minute : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Sexagesimal;

    public TimeSpan MinSize => TimeSpan.FromMinutes(1);

    public DateTime Snap(DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

    public string GetDateTimeFormatString()
        => $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";

    public DateTime Next(DateTime dateTime, int increment = 1) => dateTime.AddMinutes(increment);
}
