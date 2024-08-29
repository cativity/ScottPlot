namespace ScottPlot.Control;

public class MouseState
{
    /// <summary>
    ///     A click-drag must exceed this number of pixels before it is considered a drag.
    /// </summary>
    public float MinimumDragDistance { get; set; } = 5;

    private readonly HashSet<MouseButton> _pressed = [];

    public Pixel LastPosition = new Pixel(float.NaN, float.NaN);

    public void Down(MouseButton button) => _pressed.Add(button);

    public bool IsDown(MouseButton button) => _pressed.Contains(button);

    public IReadOnlyCollection<MouseButton> PressedButtons => [.. _pressed];

    public Pixel MouseDownPosition { get; private set; }

    public readonly MultiAxisLimitManager MouseDownAxisLimits = new MultiAxisLimitManager();

    public void Up(MouseButton button)
    {
        ForgetMouseDown();
        _pressed.Remove(button);
    }

    public void Down(Pixel position, MouseButton button, MultiAxisLimitManager limits)
    {
        RememberMouseDown(position, limits);
        Down(button);
    }

    private void RememberMouseDown(Pixel position, MultiAxisLimitManager limits)
    {
        MouseDownPosition = position;
        MouseDownAxisLimits.Remember(limits);
    }

    private void ForgetMouseDown()
    {
        MouseDownPosition = Pixel.NaN;
    }

    public bool IsDragging(Pixel position)
    {
        if (float.IsNaN(MouseDownPosition.X))
        {
            return false;
        }

        Pixel pixelDifference = MouseDownPosition - position;
        PixelSize ps = new PixelSize(pixelDifference.X, pixelDifference.Y);

        return ps.Diagonal > MinimumDragDistance;
    }
}
