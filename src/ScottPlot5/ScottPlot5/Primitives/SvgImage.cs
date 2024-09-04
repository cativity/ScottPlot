namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool _isDisposed;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream _stream;

    public SKCanvas Canvas => _canvas ?? throw new InvalidOperationException("Canvas can NOT be access after rendering!");

    private SKCanvas? _canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        _stream = new MemoryStream();
        SKRect rect = new SKRect(0, 0, Width, Height);
        _canvas = SKSvgCanvas.Create(rect, _stream);
    }

    ~SvgImage()
    {
        // Release unmanaged resources if the Dispose method wasn't called explicitly
        Dispose(false);
    }

    public string GetXml()
    {
        // The SVG document is not guaranteed to be valid until the canvas is disposed
        // See: https://learn.microsoft.com/en-us/dotnet/api/skiasharp.sksvgcanvas?view=skiasharp-2.88#remarks
        _canvas?.Dispose();
        // Canvas no more relevant
        _canvas = null;

        return Encoding.UTF8.GetString(_stream.ToArray());
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent the finalizer from running
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed resources
            _canvas?.Dispose();
            _stream.Dispose();
        }

        // Dispose unmanaged resources

        _isDisposed = true;
    }
}
