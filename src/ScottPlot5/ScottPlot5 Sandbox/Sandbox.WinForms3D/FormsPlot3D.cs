using Sandbox.WinForms3D.Primitives3D;
using SkiaSharp.Views.Desktop;

namespace Sandbox.WinForms3D;

public class FormsPlot3D : UserControl
{
    private readonly SKControl _skControl;

    public readonly Plot3D Plot3D = new Plot3D();

    private Rotation3D? _mouseDownRotation;
    private double _mouseDownZoom;
    private Point _mouseDownPoint;
    private Point3D _mouseDownCameraCenter;

    public FormsPlot3D()
    {
        _skControl = new SKControl { Dock = DockStyle.Fill };
        _skControl.PaintSurface += (s, e) => Plot3D.Render(e.Surface);
        Controls.Add(_skControl);

        _skControl.MouseDown += (s, e) =>
        {
            _mouseDownRotation = Plot3D.Rotation;
            _mouseDownPoint = e.Location;
            _mouseDownZoom = Plot3D.ZoomFactor;
            _mouseDownCameraCenter = Plot3D.CameraCenter;
        };

        _skControl.MouseUp += (s, e) => _mouseDownRotation = null;

        _skControl.MouseMove += (s, e) =>
        {
            if (_mouseDownRotation is null)
            {
                return;
            }

            int dX = e.X - _mouseDownPoint.X;
            int dY = e.Y - _mouseDownPoint.Y;

            Plot3D.Rotation = _mouseDownRotation.Value;

            if (e.Button == MouseButtons.Left)
            {
                const float rotateSensitivity = 0.2f;
                Plot3D.Rotation.DegreesY += -dX * rotateSensitivity;
                Plot3D.Rotation.DegreesX += dY * rotateSensitivity;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                const float panSensitivity = 0.005f;
                Plot3D.CameraCenter.X = _mouseDownCameraCenter.X - (dX * panSensitivity);
                Plot3D.CameraCenter.Y = _mouseDownCameraCenter.Y + (dY * panSensitivity);
            }
            else if (e.Button == MouseButtons.Right)
            {
                float dMax = Math.Max(dX, -dY);
                Plot3D.ZoomFactor = _mouseDownZoom + dMax;
            }

            Refresh();
        };
    }

    public override void Refresh()
    {
        _skControl.Invalidate();
        base.Refresh();
    }
}
