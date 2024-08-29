﻿using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using Avalonia;

namespace ScottPlot.Avalonia;

public class AvaPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";

    public List<ContextMenuItem> ContextMenuItems { get; set; } = new List<ContextMenuItem>();

    private readonly AvaPlot ThisControl;

    public AvaPlotMenu(AvaPlot avaPlot)
    {
        ThisControl = avaPlot;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new ContextMenuItem { Label = "Save Image", OnInvoke = OpenSaveImageDialog };

        // TODO: Copying images to the clipboard is still difficult in Avalonia
        // https://github.com/AvaloniaUI/Avalonia/issues/3588

        ContextMenuItem autoscale = new ContextMenuItem { Label = "Autoscale", OnInvoke = Autoscale, };

        return new[] { saveImage, autoscale, };
    }

    public ContextMenu GetContextMenu()
    {
        List<MenuItem> items = new List<MenuItem>();

        foreach (ContextMenuItem curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                items.Add(new MenuItem { Header = "-" });
            }
            else
            {
                MenuItem? menuItem = new MenuItem { Header = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(ThisControl);
                items.Add(menuItem);
            }
        }

        return new ContextMenu { ItemsSource = items };
    }

    public async void OpenSaveImageDialog(IPlotControl plotControl)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(ThisControl) ?? throw new NullReferenceException("Could not find a top level");

        IStorageFile? destinationFile = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            SuggestedFileName = DefaultSaveImageFilename,
            FileTypeChoices = filePickerFileTypes
        });

        string? path = destinationFile?.TryGetLocalPath();

        if (path is not null && !string.IsNullOrWhiteSpace(path))
        {
            PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
            plotControl.Plot.Save(path, (int)lastRenderSize.Width, (int)lastRenderSize.Height, ImageFormatLookup.FromFilePath(path));
        }
    }

    public readonly List<FilePickerFileType> filePickerFileTypes = new List<FilePickerFileType>
    {
        new FilePickerFileType("PNG Files") { Patterns = new List<string> { "*.png" } },
        new FilePickerFileType("JPEG Files") { Patterns = new List<string> { "*.jpg", "*.jpeg" } },
        new FilePickerFileType("BMP Files") { Patterns = new List<string> { "*.bmp" } },
        new FilePickerFileType("WebP Files") { Patterns = new List<string> { "*.webp" } },
        new FilePickerFileType("SVG Files") { Patterns = new List<string> { "*.svg" } },
        new FilePickerFileType("All Files") { Patterns = new List<string> { "*" } },
    };

    public static void Autoscale(IPlotControl plotControl)
    {
        plotControl.Plot.Axes.AutoScale();
        plotControl.Refresh();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        ContextMenu? manualContextMenu = GetContextMenu();

        // I am fully aware of how janky it is to place the menu in a 1x1 rect,
        // unfortunately the Avalonia docs were down when I wrote this
        manualContextMenu.PlacementRect = new Rect(pixel.X, pixel.Y, 1, 1);

        manualContextMenu.Open(ThisControl);
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

    public void Add(string Label, Action<IPlotControl> action)
    {
        ContextMenuItems.Add(new ContextMenuItem() { Label = Label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem() { IsSeparator = true });
    }
}
