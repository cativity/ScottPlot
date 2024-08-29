using System.Runtime.InteropServices;

namespace ScottPlot;

public static class Platform
{
    private enum OS
    {
        Windows,
        Linux,
        MacOS,
        Other,
    }

    private static readonly OS _thisOS = GetOS();

    private static OS GetOS()
    {
#if NETFRAMEWORK
        return OS.Windows;
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OS.Windows;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OS.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OS.MacOS;
        }

        return OS.Other;
#endif
    }

    /// <summary>
    ///     Launch a web browser to a URL using a command appropriate for the operating system
    /// </summary>
    public static void LaunchWebBrowser(string url)
    {
        switch (_thisOS)
        {
            case OS.Windows:
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });

                break;

            case OS.Linux:
                Process.Start("xdg-open", url);

                break;

            case OS.MacOS:
                Process.Start("open", url);

                break;

            case OS.Other:
                throw new InvalidOperationException("Cannot launch a web browser on this OS");

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void LaunchFile(string filePath)
    {
        filePath = Path.GetFullPath(filePath);
        ProcessStartInfo psi = new ProcessStartInfo(filePath) { UseShellExecute = true };
        Process.Start(psi);
    }
}
