using System.Diagnostics;
using System.Runtime.InteropServices;
using ScottPlot;
using WinForms_Demo.Properties;
using Color = System.Drawing.Color;

namespace WinForms_Demo;

public partial class DemoWindowInfo : UserControl
{
    [DllImport("user32.dll")]
    private static extern bool HideCaret(IntPtr hWnd);

    private readonly Type _formType;

    public DemoWindowInfo(IDemoWindow demoForm, Type formType)
    {
        InitializeComponent();
        _formType = formType;
        richTextBox1.Click += (_, _) => HideCaret(richTextBox1.Handle);
        groupBox1.Text = demoForm.Title;
        richTextBox1.Text = demoForm.Description;
        button1.Click += (_, _) => LaunchDemoWindow(demoForm.Title);
        label1.Cursor = Cursors.Hand;
        label1.MouseEnter += (_, _) => label1.ForeColor = Color.Blue;
        label1.MouseLeave += (_, _) => label1.ForeColor = SystemColors.ControlDark;
        label1.Click += (_, _) => LaunchSourceBrowser();
        HideCaret(richTextBox1.Handle);
        richTextBox1.Enabled = false;
    }

    private void LaunchSourceBrowser()
    {
        const string folder = "https://github.com/ScottPlot/ScottPlot/tree/main/src/ScottPlot5/ScottPlot5%20Demos/ScottPlot5%20WinForms%20Demo/Demos/";
        string filename = _formType.Name + ".cs";
        string url = folder + filename;
        Platform.LaunchWebBrowser(url);
    }

    private void LaunchDemoWindow(string title)
    {
        Form? form = Activator.CreateInstance(_formType) as Form;
        Debug.Assert(form is not null);
        form.Icon = Resources.scottplot_icon_rounded_border_ico;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = title;
        ParentForm?.Hide();
        form.ShowDialog();
        ParentForm?.Show();
    }
}
