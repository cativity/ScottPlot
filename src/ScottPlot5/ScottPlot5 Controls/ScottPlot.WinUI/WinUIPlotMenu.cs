using Microsoft.UI.Xaml.Controls;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace ScottPlot.WinUI;

public class WinUIPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";

    public List<ContextMenuItem> ContextMenuItems { get; set; } = new List<ContextMenuItem>();

    private readonly WinUIPlot _thisControl;

    public WinUIPlotMenu(WinUIPlot thisControl)
    {
        _thisControl = thisControl;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new ContextMenuItem { Label = "Save Image", OnInvoke = OpenSaveImageDialog, };

        ContextMenuItem copyImage = new ContextMenuItem { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard, };

        ContextMenuItem autoscale = new ContextMenuItem { Label = "Autoscale", OnInvoke = Autoscale, };

        return new[] { saveImage, copyImage, autoscale, };
    }

    public MenuFlyout GetContextMenu(IPlotControl plotControl)
    {
        MenuFlyout menu = new MenuFlyout();

        foreach (ContextMenuItem curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.Add(new MenuFlyoutSeparator());
            }
            else
            {
                MenuFlyoutItem menuItem = new MenuFlyoutItem { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(plotControl);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public async void OpenSaveImageDialog(IPlotControl plotControl)
    {
        FileSavePicker dialog = new FileSavePicker { SuggestedFileName = DefaultSaveImageFilename };
        dialog.FileTypeChoices.Add("PNG Files", new List<string>() { ".png" });
        dialog.FileTypeChoices.Add("JPEG Files", new List<string>() { ".jpg", ".jpeg" });
        dialog.FileTypeChoices.Add("BMP Files", new List<string>() { ".bmp" });
        dialog.FileTypeChoices.Add("WebP Files", new List<string>() { ".webp" });
        dialog.FileTypeChoices.Add("SVG Files", new List<string>() { ".svg" });

#if NET6_0_WINDOWS10_0_18362 // https://github.com/microsoft/CsWinRT/blob/master/docs/interop.md#windows-sdk
        // TODO: launch a pop-up window or otherwise inform if AppWindow is not set before using save-dialog
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(ThisControl.AppWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(dialog, hwnd);
#endif

        StorageFile? file = await dialog.PickSaveFileAsync();

        if (file is not null)
        {
            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormatLookup.FromFilePath(file.Name);
            PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
            plotControl.Plot.Save(file.Path, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
        }
    }

    public static void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        byte[] bytes = plotControl.Plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height).GetImageBytes();

        InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
        stream.AsStreamForWrite().Write(bytes);

        DataPackage content = new DataPackage();
        content.SetBitmap(RandomAccessStreamReference.CreateFromStream(stream));

        Clipboard.SetContent(content);
    }

    public static void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        MenuFlyout flyout = GetContextMenu(_thisControl);
        Point pt = new Point(pixel.X, pixel.Y);
        flyout.ShowAt(_thisControl, pt);
    }

    public void Reset()
    {
        Clear();
        ContextMenuItems.AddRange(GetDefaultContextMenuItems());
    }

    public void Clear()
    {
        ContextMenuItems.Clear();
    }

    public void Add(string label, Action<IPlotControl> action)
    {
        ContextMenuItems.Add(new ContextMenuItem() { Label = label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem() { IsSeparator = true });
    }
}
