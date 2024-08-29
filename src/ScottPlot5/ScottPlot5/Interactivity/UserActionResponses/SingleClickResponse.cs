namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickResponse(MouseButton button, Action<Plot, Pixel> action) : IUserActionResponse
{
    /// <summary>
    ///     Which mouse button to watch for single-click events
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    /// <summary>
    ///     This action is invoked when a single-click occurs.
    /// </summary>
    public Action<Plot, Pixel> ResponseAction { get; } = action;

    /// <summary>
    ///     Location of the previous mouse down event
    /// </summary>
    private Pixel _mouseDownPixel = Pixel.NaN;

    public ResponseInfo Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction buttonAction && buttonAction.Button == MouseButton)
        {
            if (buttonAction.IsPressed)
            {
                _mouseDownPixel = buttonAction.Pixel;

                return ResponseInfo.NoActionRequired;
            }

            if (double.IsNaN(_mouseDownPixel.X))
            {
                return ResponseInfo.NoActionRequired;
            }

            // do not respond to mouse dragging
            double dX = Math.Abs(_mouseDownPixel.X - buttonAction.Pixel.X);
            double dY = Math.Abs(_mouseDownPixel.Y - buttonAction.Pixel.Y);
            double rightClickDragDistance = Math.Max(dX, dY);

            if (rightClickDragDistance >= 5)
            {
                _mouseDownPixel = Pixel.NaN;

                return ResponseInfo.NoActionRequired;
            }

            ResponseAction.Invoke(plot, buttonAction.Pixel);
            _mouseDownPixel = Pixel.NaN;

            return new ResponseInfo { RefreshNeeded = true };
        }

        return ResponseInfo.NoActionRequired;
    }
}
