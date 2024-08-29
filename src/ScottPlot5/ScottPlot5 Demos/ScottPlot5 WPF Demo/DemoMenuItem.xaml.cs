using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Demo;

public partial class DemoMenuItem : UserControl
{
    public DemoMenuItem()
    {
        InitializeComponent();
    }

    public DemoMenuItem(Type type)
    {
        object? instance = Activator.CreateInstance(type) as IDemoWindow;
        InitializeComponent();
        Debug.Assert(instance is not null);
        GroupBox1.Header = ((IDemoWindow)instance).DemoTitle;
        TextBlock1.Text = ((IDemoWindow)instance).Description;
        ((Window)instance).Close();

        Button1.Click += (s, e) => ((Window)Activator.CreateInstance(type)!).Show();
    }
}
