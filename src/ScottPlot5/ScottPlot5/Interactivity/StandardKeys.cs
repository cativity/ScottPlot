namespace ScottPlot.Interactivity;

/// <summary>
///     Structures for commonly used keys.
///     Use these as a safer alternative to instantiating your own.
/// </summary>
public static class StandardKeys
{
    public static Key Alt { get; } = new Key("alt");

    public static Key Control { get; } = new Key("ctrl");

    public static Key Shift { get; } = new Key("shift");

    public static Key Down { get; } = new Key("down");

    public static Key Up { get; } = new Key("up");

    public static Key Left { get; } = new Key("left");

    public static Key Right { get; } = new Key("right");

    public static Key Unknown { get; } = new Key("unknown");

    public static Key A { get; } = new Key("a");

    public static bool IsArrowKey(Key key) => key == Left || key == Right || key == Down || key == Up;
}
