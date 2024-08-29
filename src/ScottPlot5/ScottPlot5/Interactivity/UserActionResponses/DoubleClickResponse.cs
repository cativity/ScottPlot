namespace ScottPlot.Interactivity.UserActionResponses;

public class DoubleClickResponse(MouseButton button, Action<Plot, Pixel> action) : IUserActionResponse
{
    /// <summary>
    ///     Which mouse button to watch for double-clicks.
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    /// <summary>
    ///     This action is invoked when a double-click occurs.
    ///     Replace this action with your own logic to customize double-click behavior.
    /// </summary>
    public Action<Plot, Pixel> ResponseAction { get; } = action;

    /// <summary>
    ///     Consecutive clicks are only considered a double-click if the time between the first
    ///     click mouse down and second click mouse up does not exceed this value.
    /// </summary>
    public TimeSpan MaximumTimeBetweenClicks = TimeSpan.FromMilliseconds(500);

    private DateTime _latestMouseDownTime = DateTime.MinValue;

    private DateTime _previousMouseDownTime = DateTime.MinValue;

    public ResponseInfo Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction mouseAction && mouseAction.Button == MouseButton)
        {
            if (mouseAction.IsPressed)
            {
                _previousMouseDownTime = _latestMouseDownTime;
                _latestMouseDownTime = mouseAction.DateTime;

                return ResponseInfo.NoActionRequired;
            }

            TimeSpan timeSinceFirstMouseDown = mouseAction.DateTime - _previousMouseDownTime;

            if (timeSinceFirstMouseDown < MaximumTimeBetweenClicks)
            {
                ResponseAction.Invoke(plot, mouseAction.Pixel);
                _latestMouseDownTime = DateTime.MinValue; // reset time so a third click won't toggle it back

                return ResponseInfo.Refresh;
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
