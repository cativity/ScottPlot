using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF_Demo.DemoWindows;
using Version = ScottPlot.Version;

namespace WPF_Demo;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string logoImagePath = Path.GetFullPath("Resources/logo-128.png");
        Uri logoImageUri = new Uri(logoImagePath, UriKind.Absolute);
        LogoImage.Source = new BitmapImage(logoImageUri);

        Subtitle.Content = $"ScottPlot.WPF Version {Version.VersionString}";

        DemoItemPanel.Children.Clear();

        List<Type> demoWindows = Assembly.GetAssembly(typeof(MainWindow))?.GetTypes()
                                         .Where(x => x.IsAssignableTo(typeof(IDemoWindow)) && !x.IsInterface)
                                         .ToList() ?? [];

        MoveToTop(typeof(Quickstart));

        demoWindows.ForEach(x => DemoItemPanel.Children.Add(new DemoMenuItem(x)));

        return;

        void MoveToTop(Type t)
        {
            demoWindows.Remove(t);
            demoWindows.Insert(0, t);
        }
    }
}
