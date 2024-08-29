namespace ScottPlot.Interactivity;

/// <summary>
///     Tracks which keyboard keys are currently pressed.
/// </summary>
public class KeyboardState
{
    private readonly HashSet<string> _pressedKeyNames = [];

    public int PressedKeyCount => _pressedKeyNames.Count;

    public void Reset()
    {
        _pressedKeyNames.Clear();
    }

    public void Add(Key key)
    {
        _pressedKeyNames.Add(key.Name);
    }

    public void Remove(Key key)
    {
        _pressedKeyNames.Remove(key.Name);
    }

    public bool IsPressed(Key key) => IsPressed(key.Name);

    public bool IsPressed(string keyName) => _pressedKeyNames.Contains(keyName);

    public string[] GetPressedKeyNames => [.. _pressedKeyNames];

    public override string ToString()
    {
        return _pressedKeyNames.Count == 0
                   ? "KeyState with 0 pressed key"
                   : $"KeyState with {_pressedKeyNames.Count} pressed keys: " + string.Join(", ", _pressedKeyNames.Select(static x => x));
    }
}
