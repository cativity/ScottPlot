using ScottPlot.Control;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScottPlot.WinForms;

public class FormsPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";

    public List<ContextMenuItem> ContextMenuItems { get; set; } = new List<ContextMenuItem>();

    private readonly FormsPlotBase _thisControl;

    public FormsPlotMenu(FormsPlotBase control)
    {
        _thisControl = control;
        Reset();
    }

    public List<ContextMenuItem> StandardContextMenuItems()
    {
        ContextMenuItem saveImage = new ContextMenuItem { Label = "Save Image", OnInvoke = OpenSaveImageDialog, };

        ContextMenuItem copyImage = new ContextMenuItem { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard, };

        ContextMenuItem autoscale = new ContextMenuItem { Label = "Autoscale", OnInvoke = Autoscale, };

        ContextMenuItem newWindow = new ContextMenuItem { Label = "Open in New Window", OnInvoke = OpenInNewWindow, };

        return new List<ContextMenuItem> { saveImage, copyImage, autoscale, newWindow, };
    }

    public static void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        Bitmap bmp = plotControl.Plot.GetBitmap((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        Clipboard.SetImage(bmp);
    }

    public static void OpenInNewWindow(IPlotControl plotControl)
    {
        FormsPlotViewer.Launch(plotControl.Plot, "Interactive Plot");
        plotControl.Refresh();
    }

    public static void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public ContextMenuStrip GetContextMenu()
    {
        ContextMenuStrip menu = new ContextMenuStrip();

        foreach (ContextMenuItem item in ContextMenuItems)
        {
            if (item.IsSeparator)
            {
                menu.Items.Add(new ToolStripSeparator());
            }
            else
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Label);
                menuItem.Click += (_, _) => item.OnInvoke(_thisControl);

                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public void OpenSaveImageDialog(IPlotControl plotControl)
    {
        SaveFileDialog dialog = new SaveFileDialog
        {
            FileName = DefaultSaveImageFilename,
            Filter = "PNG Files (*.png)|*.png"
                     + "|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg"
                     + "|BMP Files (*.bmp)|*.bmp"
                     + "|WebP Files (*.webp)|*.webp"
                     + "|SVG Files (*.svg)|*.svg"
                     + "|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            ImageFormat format;

            try
            {
                format = ImageFormatLookup.FromFilePath(dialog.FileName);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
                plotControl.Plot.Save(dialog.FileName, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void ShowContextMenu(Pixel pixel)
    {
        Debug.WriteLine("Showing Context Menu");
        ContextMenuStrip menu = GetContextMenu();
        menu.Show(_thisControl, new Point((int)pixel.X, (int)pixel.Y));
    }

    public void Reset()
    {
        Clear();
        ContextMenuItems.AddRange(StandardContextMenuItems());
    }

    public void Clear()
    {
        ContextMenuItems.Clear();
    }

    public void Add(string label, Action<IPlotControl> action)
    {
        ContextMenuItems.Add(new ContextMenuItem { Label = label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem { IsSeparator = true });
    }
}
