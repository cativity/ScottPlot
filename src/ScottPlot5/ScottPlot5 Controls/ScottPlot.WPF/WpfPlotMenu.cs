﻿using Microsoft.Win32;
using ScottPlot.Control;
using System.Windows.Media.Imaging;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;
using System.Windows.Controls.Primitives;

namespace ScottPlot.WPF;

public class WpfPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";

    public List<ContextMenuItem> ContextMenuItems { get; set; } = new List<ContextMenuItem>();

    private readonly WpfPlotBase _thisControl;

    public WpfPlotMenu(WpfPlotBase control)
    {
        _thisControl = control;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new ContextMenuItem { Label = "Save Image", OnInvoke = OpenSaveImageDialog };

        ContextMenuItem copyImage = new ContextMenuItem { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

        ContextMenuItem autoscale = new ContextMenuItem { Label = "Autoscale", OnInvoke = Autoscale, };

        ContextMenuItem newWindow = new ContextMenuItem { Label = "Open in New Window", OnInvoke = OpenInNewWindow, };

        return new[] { saveImage, copyImage, newWindow, };
    }

    public ContextMenu GetContextMenu()
    {
        ContextMenu menu = new ContextMenu();

        foreach (ContextMenuItem curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.Add(new Separator());
            }
            else
            {
                MenuItem menuItem = new MenuItem { Header = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(_thisControl);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public void ShowContextMenu(Pixel position)
    {
        ContextMenu? menu = GetContextMenu();
        menu.PlacementTarget = _thisControl;
        menu.Placement = PlacementMode.MousePoint;
        menu.IsOpen = true;
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

        if (dialog.ShowDialog() is true)
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
                MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            try
            {
                PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
                plotControl.Plot.Save(dialog.FileName, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
        }
    }

    public static void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        Image bmp = plotControl.Plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        byte[] bmpBytes = bmp.GetImageBytes();

        using MemoryStream ms = new MemoryStream();
        ms.Write(bmpBytes, 0, bmpBytes.Length);
        BitmapImage bmpImage = new BitmapImage();
        bmpImage.BeginInit();
        bmpImage.StreamSource = ms;
        bmpImage.EndInit();
        Clipboard.SetImage(bmpImage);
    }

    public static void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public static void OpenInNewWindow(IPlotControl plotControl)
    {
        WpfPlotViewer.Launch(plotControl.Plot, "Interactive Plot");
        plotControl.Refresh();
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
