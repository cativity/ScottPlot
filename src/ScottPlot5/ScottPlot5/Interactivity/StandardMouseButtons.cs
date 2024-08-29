namespace ScottPlot.Interactivity;

/// <summary>
///     Structures for commonly used mouse buttons.
///     Use these as a safer alternative to instantiating your own.
/// </summary>
public static class StandardMouseButtons
{
    public static MouseButton Left { get; } = new MouseButton("left");

    public static MouseButton Right { get; } = new MouseButton("right");

    public static MouseButton Middle { get; } = new MouseButton("middle");

    public static MouseButton Wheel { get; } = new MouseButton("wheel");
}
