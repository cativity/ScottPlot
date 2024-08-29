namespace ScottPlot.Control;

public class KeyboardState
{
    private readonly HashSet<Key> _pressed = [];

    public IReadOnlyCollection<Key> PressedKeys => [.. _pressed];

    public void Down(Key key)
    {
        if (key == Key.Unknown)
        {
            return;
        }

        _pressed.Add(key);
    }

    public void Up(Key key)
    {
        if (key == Key.Unknown)
        {
            return;
        }

        _pressed.Remove(key);
    }
}
