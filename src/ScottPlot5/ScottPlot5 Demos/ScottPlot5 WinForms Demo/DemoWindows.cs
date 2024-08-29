using System.Reflection;
using System.Runtime.CompilerServices;
using WinForms_Demo.Demos;

namespace WinForms_Demo;

public static class DemoWindows
{
    public static List<IDemoWindow> GetDemoWindows()
    {
        List<IDemoWindow> windows = Assembly.GetExecutingAssembly()
                                            .GetTypes()
                                            .Where(static x => !x.IsAbstract && x.IsSubclassOf(typeof(Form)) && x.GetInterfaces().Contains(typeof(IDemoWindow)))
                                            .Select(static x => RuntimeHelpers.GetUninitializedObject(x) as IDemoWindow)
                                            .OfType<IDemoWindow>()
                                            .ToList();

        MoveToTop(typeof(DraggablePoints));
        MoveToTop(typeof(DraggableAxisLines));
        MoveToTop(typeof(MouseTracker));
        MoveToTop(typeof(CookbookViewer));

        MoveToBottom(typeof(OpenGL));

        return windows;

        void MoveToTop(Type targetType)
        {
            IDemoWindow targetWindow = windows.Single(x => x.GetType() == targetType);
            windows.Remove(targetWindow);
            windows.Insert(0, targetWindow);
        }

        void MoveToBottom(Type targetType)
        {
            IDemoWindow targetWindow = windows.Single(x => x.GetType() == targetType);
            windows.Remove(targetWindow);
            windows.Add(targetWindow);
        }
    }
}
