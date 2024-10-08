﻿namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseDragZoom(MouseButton button) : IUserActionResponse
{
    private Pixel _mouseDownPixel = Pixel.NaN;

    private MouseButton MouseButton { get; } = button;

    // TODO: re-implement this being more careful about allocations
    private MultiAxisLimits? _rememberedLimits;

    public ResponseInfo Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is IMouseButtonAction mouseDownAction && mouseDownAction.Button == MouseButton && mouseDownAction.IsPressed)
        {
            _mouseDownPixel = mouseDownAction.Pixel;
            _rememberedLimits = new MultiAxisLimits(plot);

            return new ResponseInfo { IsPrimary = false };
        }

        if (_mouseDownPixel == Pixel.NaN)
        {
            return ResponseInfo.NoActionRequired;
        }

        if (userInput is IMouseButtonAction mouseUpAction && mouseUpAction.Button == MouseButton && !mouseUpAction.IsPressed)
        {
            _rememberedLimits?.Recall();
            ApplyToPlot(plot, _mouseDownPixel, mouseUpAction.Pixel, keys);
            _mouseDownPixel = Pixel.NaN;

            return ResponseInfo.Refresh;
        }

        if (userInput is IMouseAction mouseMoveAction)
        {
            _rememberedLimits?.Recall();
            ApplyToPlot(plot, _mouseDownPixel, mouseMoveAction.Pixel, keys);

            return new ResponseInfo { RefreshNeeded = true, IsPrimary = true };
        }

        return new ResponseInfo { IsPrimary = true };
    }

    private static void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        if (keys.IsPressed(StandardKeys.Shift))
        {
            px2.X = px1.X;
        }

        if (keys.IsPressed(StandardKeys.Control))
        {
            px2.Y = px1.Y;
        }

        MouseAxisManipulation.DragZoom(plot, px1, px2);
    }
}
