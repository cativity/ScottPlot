using System.Diagnostics;
using Version = ScottPlot.Version;

namespace WinForms_Demo;

public partial class MainMenuForm : Form
{
    private readonly HashSet<Keys> _pressedKeys = [];

    public MainMenuForm(Type? launchFormType)
    {
        InitializeComponent();

        // launch a test window if CTRL+D is pressed at startup
        KeyPreview = true;
        KeyUp += (_, e) => _pressedKeys.Remove(e.KeyCode);

        KeyDown += (_, e) =>
        {
            _pressedKeys.Add(e.KeyCode);

            if (launchFormType is not null && _pressedKeys.Contains(Keys.ControlKey) && _pressedKeys.Contains(Keys.D))
            {
                Hide();
                Form? form = Activator.CreateInstance(launchFormType) as Form;
                Debug.Assert(form is not null);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
                Close();
            }
        };

        label1.Text = "ScottPlot Demo";
        label2.Text = "ScottPlot.WinForms " + Version.VersionString;
        Text = $"{Version.LongString} Demo";

        tbSearch.TextChanged += (_, _) => demoWindowScrollList1.Update(tbSearch.Text, null);
        Shown += (_, _) => demoWindowScrollList1.Update(tbSearch.Text, null);
    }
}
