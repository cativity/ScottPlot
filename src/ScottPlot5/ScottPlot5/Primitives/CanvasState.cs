namespace ScottPlot;

/// <summary>
///     This object manages wraps a SKCanvas and manages calls
///     to SkiaSharp methods to ensure state is tracked across
///     complex render systems.
/// </summary>
public class CanvasState(SKCanvas canvas)
{
    /// <summary>
    ///     Number of times <see cref="Save" /> was called without <see cref="Restore" />
    /// </summary>
    public int SaveLevels { get; set; }

    /// <summary>
    ///     Save the current state of the canvas.
    ///     This state can be recalled by calling <see cref="Restore" />.
    /// </summary>
    public void Save()
    {
        canvas.Save();
        SaveLevels++;
    }

    /// <summary>
    ///     Restore the canvas to the state the last time <see cref="Save" /> was called.
    ///     This method will throw if a canvas is restored more times than it was saved.
    ///     Use the <see cref="RestoreAll" /> method to restore a canvas to its
    ///     original state if the number of saves is unknown.
    /// </summary>
    public void Restore()
    {
        if (SaveLevels == 0)
        {
            throw new InvalidOperationException("Restore() was called more than Save()");
        }

        canvas.Restore();
        SaveLevels--;
    }

    /// <summary>
    ///     Restore the canvas to its original state, regardless
    ///     of how many times <see cref="Save" /> was called.
    /// </summary>
    public void RestoreAll()
    {
        while (SaveLevels > 0)
        {
            Restore();
        }
    }

    /// <summary>
    ///     Restore the canvas to its original state.
    ///     Disables all clipping and transformations.
    /// </summary>
    public void DisableClipping()
    {
        RestoreAll();
    }

    /// <summary>
    ///     Clip the canvas so drawing will only occur within the given rectangle.
    /// </summary>
    public void Clip(PixelRect rect)
    {
        canvas.ClipRect(rect.ToSKRect());
    }

    public void Translate(Pixel px)
    {
        canvas.Translate(px.X, px.Y);
    }

    public void RotateDegrees(double degrees)
    {
        canvas.RotateDegrees((float)degrees, 0, 0);
    }

    public void RotateDegrees(double degrees, Pixel px)
    {
        canvas.RotateDegrees((float)degrees, px.X, px.Y);
    }

    public void RotateRadians(double radians)
    {
        canvas.RotateRadians((float)radians, 0, 0);
    }

    public void RotateRadians(double radians, Pixel px)
    {
        canvas.RotateRadians((float)radians, px.X, px.Y);
    }
}
