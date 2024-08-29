using Eto;
using Eto.Forms;

namespace Sandbox.Eto;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        new Application(Platform.Detect).Run(new MainWindow());
    }
}
