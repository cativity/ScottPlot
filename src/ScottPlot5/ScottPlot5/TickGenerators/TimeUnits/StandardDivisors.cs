namespace ScottPlot.TickGenerators.TimeUnits;

internal static class StandardDivisors
{
    public static IReadOnlyList<int> Decimal { get; } = [1, 5, 10];

    public static IReadOnlyList<int> Sexagesimal { get; } = [1, 5, 10, 15, 20, 30, 60];

    public static IReadOnlyList<int> Dozenal { get; } = [1, 2, 3, 4, 6, 12];

    public static IReadOnlyList<int> Hexadecimal { get; } = [1, 2, 3, 4, 6, 8, 16];

    public static IReadOnlyList<int> Days { get; } = [1, 3, 7, 14, 28];

    public static IReadOnlyList<int> Months { get; } = [1, 3, 6];

    public static IReadOnlyList<int> Years { get; } = [1, 2, 3, 4, 5, 10];
}
