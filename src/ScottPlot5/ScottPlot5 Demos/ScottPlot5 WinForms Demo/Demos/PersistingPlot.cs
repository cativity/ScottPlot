using JetBrains.Annotations;
using ScottPlot;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class PersistingPlot : Form, IDemoWindow
{
    public string Title => "Persisting Plot";

    public string Description => "Manipulations to a Plot on another Form persist through Close() events";

    private readonly ExamplePlotForm _persistentForm = new ExamplePlotForm();

    public PersistingPlot()
    {
        InitializeComponent();

        _persistentForm.FormsPlot1.Plot.Add.Signal(Generate.RandomWalk(100));

        button1.Click += (_, _) => _persistentForm.ShowDialog();
    }
}
