using WinForms_Demo.Demos;

namespace WinForms_Demo;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();

        // CTRL+D opens this window (useful for testing in development)
        Type testingFormType = typeof(CustomMouseActions);

        Application.Run(new MainMenuForm(testingFormType));
    }
}
