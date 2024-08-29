using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using SkiaSharp.Views.Desktop;
using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
#if NETCOREAPP || NET
using OpenTK.Mathematics;
#endif

#nullable disable

namespace SkiaSharp.Views.WPF;

[DefaultEvent("PaintSurface")]
[DefaultProperty("Name")]
public class SKGLElement : GLWpfControl, IDisposable
{
    private const SKColorType _colorType = SKColorType.Rgba8888;
    private const GRSurfaceOrigin _surfaceOrigin = GRSurfaceOrigin.BottomLeft;

    private bool _designMode;

    private GRContext _grContext;
    private GRGlFramebufferInfo _glInfo;
    private GRBackendRenderTarget _renderTarget;
    private SKSurface _surface;
    private SKCanvas _canvas;

    private SKSizeI _lastSize;
    private SKGLElementWindowListener _listener;

    public SKGLElement()
        : base()
    {
        Initialize();
    }

    private void Initialize()
    {
        _designMode = DesignerProperties.GetIsInDesignMode(this);
        //#if NETCOREAPP || NET
        //            RegisterToEventsDirectly = false;
        //            CanInvokeOnHandledEvents = false;
        //#endif
        GLWpfControlSettings settings = new GLWpfControlSettings() { MajorVersion = 2, MinorVersion = 1, RenderContinuously = false };

        Render += OnPaint;

        Loaded += (s, e) =>
        {
            SKGLElementWindowListener listener = new SKGLElementWindowListener(this);
            _listener = listener;
        };

        Start(settings);
    }

    private class SKGLElementWindowListener : IDisposable
    {
        private readonly WeakReference<SKGLElement> _toDestroy;
        private WeakReference<Window> _theWindow;

        public SKGLElementWindowListener(SKGLElement toDestroy)
        {
            _toDestroy = new WeakReference<SKGLElement>(toDestroy);
            Window window = Window.GetWindow(toDestroy);

            if (window is not null)
            {
                _theWindow = new WeakReference<Window>(window);
                window.Closing += Window_Closing;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_toDestroy.TryGetTarget(out SKGLElement target))
            {
                target.Dispose();
            }
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (_theWindow is not null && _theWindow.TryGetTarget(out Window window))
            {
                window.Closing -= Window_Closing;
            }

            _theWindow = null;

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public SKSize CanvasSize => _lastSize;

    public GRContext GrContext => _grContext;

    [Category("Appearance")]
    public event EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

    private SKSizeI GetSize()
    {
        double currentWidth = ActualWidth;
        double currentHeight = ActualHeight;

        if (currentWidth < 0 || currentHeight < 0)
        {
            currentWidth = 0;
            currentHeight = 0;
        }

        PresentationSource source = PresentationSource.FromVisual(this);

        double dpiX = 1.0;
        double dpiY = 1.0;

        if (source?.CompositionTarget is not null)
        {
            dpiX = source.CompositionTarget.TransformToDevice.M11;
            dpiY = source.CompositionTarget.TransformToDevice.M22;
        }

        return new SKSizeI((int)(currentWidth * dpiX), (int)(currentHeight * dpiY));
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        _grContext?.ResetContext();

        base.OnRender(drawingContext);
    }

    protected virtual void OnPaint(TimeSpan e)
    {
        if (_disposed)
        {
            return;
        }

        if (_designMode)
        {
            return;
        }

        // create the contexts if not done already
        if (_grContext is null)
        {
            GRGlInterface glInterface = GRGlInterface.Create();
            _grContext = GRContext.CreateGl(glInterface);
        }

        // get the new surface size
        SKSizeI newSize = GetSize();

        GL.ClearColor(Color4.Transparent);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        // manage the drawing surface
        if (_renderTarget is null || _lastSize != newSize || !_renderTarget.IsValid)
        {
            // create or update the dimensions
            _lastSize = newSize;

            GL.GetInteger(GetPName.FramebufferBinding, out int framebuffer);
            GL.GetInteger(GetPName.StencilBits, out int stencil);
            GL.GetInteger(GetPName.Samples, out int samples);
            int maxSamples = _grContext.GetMaxSurfaceSampleCount(_colorType);

            if (samples > maxSamples)
            {
                samples = maxSamples;
            }

            _glInfo = new GRGlFramebufferInfo((uint)framebuffer, _colorType.ToGlSizedFormat());

            // destroy the old surface
            _surface?.Dispose();
            _surface = null;
            _canvas = null;

            // re-create the render target
            _renderTarget?.Dispose();
            _renderTarget = new GRBackendRenderTarget(newSize.Width, newSize.Height, samples, stencil, _glInfo);
        }

        // create the surface
        if (_surface is null)
        {
            _surface = SKSurface.Create(_grContext, _renderTarget, _surfaceOrigin, _colorType);
            _canvas = _surface.Canvas;
        }

        using (new SKAutoCanvasRestore(_canvas, true))
        {
            // start drawing
#pragma warning disable CS0618 // Type or member is obsolete
            OnPaintSurface(new SKPaintGLSurfaceEventArgs(_surface, _renderTarget, _surfaceOrigin, _colorType, _glInfo));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        // update the control
        _canvas?.Flush();
    }

    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        // invoke the event
        PaintSurface?.Invoke(this, e);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _canvas = null;
        _surface?.Dispose();
        _surface = null;
        _renderTarget?.Dispose();
        _renderTarget = null;
        _grContext?.Dispose();
        _grContext = null;
        _listener?.Dispose();
        _listener = null;

        _disposed = true;
    }

    public new void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
