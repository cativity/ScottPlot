using Sandbox.WinForms3D.Plottables3D;
using Sandbox.WinForms3D.Primitives3D;

namespace Sandbox.WinForms3D;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Scatter3D scatter = new Scatter3D();
        formsPlot3d1.Plot3D.Plottables.Add(scatter);

        Point3D location = new Point3D(.25, .25, 0);
        Size3D size = new Size3D(.2, .2, .2);
        Rectangle3D rect = new Rectangle3D(location, size);
        formsPlot3d1.Plot3D.Plottables.Add(rect);
    }
}
