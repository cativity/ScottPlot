using Eto.Drawing;
using Eto.Forms;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScottPlot.Eto;

public class EtoPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";

    public List<ContextMenuItem> ContextMenuItems { get; set; } = new List<ContextMenuItem>();

    private readonly EtoPlot _thisControl;

    public EtoPlotMenu(EtoPlot etoPlot)
    {
        _thisControl = etoPlot;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new ContextMenuItem { Label = "Save Image", OnInvoke = OpenSaveImageDialog };

        ContextMenuItem copyImage = new ContextMenuItem { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

        ContextMenuItem autoscale = new ContextMenuItem { Label = "Autoscale", OnInvoke = Autoscale, };

        return new[] { saveImage, copyImage, autoscale, };
    }

    public ContextMenu GetContextMenu()
    {
        ContextMenu menu = new ContextMenu();

        foreach (ContextMenuItem curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.AddSeparator();
            }
            else
            {
                ButtonMenuItem? menuItem = new ButtonMenuItem() { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(_thisControl);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public readonly List<FileFilter> FileDialogFilters = new List<FileFilter>
    {
        new FileFilter { Name = "PNG Files", Extensions = new[] { "png" } },
        new FileFilter { Name = "JPEG Files", Extensions = new[] { "jpg", "jpeg" } },
        new FileFilter { Name = "BMP Files", Extensions = new[] { "bmp" } },
        new FileFilter { Name = "WebP Files", Extensions = new[] { "webp" } },
        new FileFilter { Name = "SVG Files", Extensions = new[] { "svg" } },
        new FileFilter { Name = "All Files", Extensions = new[] { "*" } },
    };

    public void OpenSaveImageDialog(IPlotControl plotControl)
    {
        SaveFileDialog dialog = new SaveFileDialog { FileName = DefaultSaveImageFilename };

        foreach (FileFilter? curr in FileDialogFilters)
        {
            dialog.Filters.Add(curr);
        }

        if (dialog.ShowDialog(_thisControl) == DialogResult.Ok)
        {
            string? filename = dialog.FileName;

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            // Eto doesn't add the extension for you when you select a filter :/
            if (!Path.HasExtension(filename))
            {
                filename += $".{dialog.CurrentFilter.Extensions[0]}";
            }

            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormatLookup.FromFilePath(filename);
            PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
            plotControl.Plot.Save(filename, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
        }
    }

    public static void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        byte[] bytes = plotControl.Plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height).GetImageBytes();
        MemoryStream ms = new MemoryStream(bytes);
        using Bitmap bmp = new Bitmap(ms);
        Clipboard.Instance.Image = bmp;
    }

    public static void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        ContextMenu? menu = GetContextMenu();
        menu.Show(_thisControl, new Point((int)pixel.X, (int)pixel.Y));
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
