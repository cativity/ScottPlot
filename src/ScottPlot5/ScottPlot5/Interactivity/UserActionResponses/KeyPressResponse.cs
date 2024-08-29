using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.UserActionResponses;

public class KeyPressResponse(Key key, Action<Plot, Pixel> action) : IUserActionResponse
{
    private Key Key { get; } = key;

    private Action<Plot, Pixel> ResponseAction { get; } = action;

    public ResponseInfo Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is KeyDown keyDownAction && keyDownAction.Key == Key)
        {
            ResponseAction.Invoke(plot, Pixel.NaN);

            return ResponseInfo.Refresh;
        }

        return ResponseInfo.NoActionRequired;
    }
}
