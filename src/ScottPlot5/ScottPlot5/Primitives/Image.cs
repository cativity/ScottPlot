using System.Runtime.InteropServices;
using ScottPlot.IO;

namespace ScottPlot;

/// <summary>
///     Bitmap representation of a <seealso cref="SkiaSharp.SKImage" />
/// </summary>
public class Image : IDisposable
{
    private bool _isDisposed;

    protected SKImage SKImage { get; }

    public int Width => SKImage.Width;

    public int Height => SKImage.Height;

    public PixelSize Size => new PixelSize(Width, Height);

    //[Obsolete("Use initializer that accepts a SKSurface", true)]
    //public Image(SKImage image) => SKImage = image;

    public Image(SKSurface surface) => SKImage = surface.Snapshot();

    public Image(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException(filename);
        }

        byte[] bytes = File.ReadAllBytes(filename);
        SKImage = SKImage.FromEncodedData(bytes);
    }

    public Image(byte[] bytes) => SKImage = SKImage.FromEncodedData(bytes);

    public Image(SKBitmap bmp) => SKImage = SKImage.FromBitmap(bmp);

    public Image(int width, int height)
        : this(width, height, Colors.Black)
    {
    }

    public Image(PixelSize size)
        : this((int)size.Width, (int)size.Height, Colors.Black)
    {
    }

    public Image(int width, int height, Color color)
    {
        SKSurface surface = SKSurface.Create(new SKImageInfo(width, height));
        SKCanvas? canvas = surface.Canvas;
        canvas.Clear(color.ToSKColor());
        SKImage = surface.Snapshot();
    }

    public Image(byte[,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);

        uint[] pixelValues = new uint[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte red = pixelArray[y, x];
                byte green = pixelArray[y, x];
                byte blue = pixelArray[y, x];
                const uint alpha = 255;

                pixelValues[(y * width) + x] = (uint)(red << 0) + (uint)(green << 8) + (uint)(blue << 16) + (alpha << 24);
            }
        }

        // https://swharden.com/csdv/skiasharp/array-to-image/
        SKBitmap bmp = new SKBitmap();
        GCHandle gcHandle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
        SKImageInfo info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = info.RowBytes;
        bmp.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });
        SKImage = SKImage.FromBitmap(bmp);
    }

    public Image(byte[,,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);

        uint[] pixelValues = new uint[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte red = pixelArray[y, x, 0];
                byte green = pixelArray[y, x, 1];
                byte blue = pixelArray[y, x, 2];
                const uint alpha = 255;

                pixelValues[(y * width) + x] = (uint)(red << 0) + (uint)(green << 8) + (uint)(blue << 16) + (alpha << 24);
            }
        }

        // https://swharden.com/csdv/skiasharp/array-to-image/
        SKBitmap bmp = new SKBitmap();
        GCHandle gcHandle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
        SKImageInfo info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = info.RowBytes;
        bmp.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });
        SKImage = SKImage.FromBitmap(bmp);
    }

    /// <summary>
    ///     SkiaSharp cannot natively create BMP files.
    ///     This function creates bitmaps in memory manually.
    ///     https://github.com/mono/SkiaSharp/issues/320
    /// </summary>
    private byte[] GetBitmapBytes()
    {
        using SKBitmap skBitmap = SKBitmap.FromImage(SKImage);

        BitmapHeader header = new BitmapHeader(Width, Height);

        byte[] bitmapBytes = new byte[skBitmap.Bytes.Length + BitmapHeader.FileHeaderSize];

        IntPtr pHeader = IntPtr.Zero;

        try
        {
            pHeader = Marshal.AllocHGlobal(BitmapHeader.FileHeaderSize);
            Marshal.StructureToPtr(header, pHeader, false);

            // copy the header from the bytes of our custom bitmap header struct
            Marshal.Copy(pHeader, bitmapBytes, 0, BitmapHeader.FileHeaderSize);

            // copy the content of the bitmap from the SkiaSharp image
            Array.Copy(skBitmap.Bytes, 0, bitmapBytes, BitmapHeader.FileHeaderSize, skBitmap.Bytes.Length);
        }
        finally
        {
            Marshal.FreeHGlobal(pHeader);
        }

        return bitmapBytes;
    }

    public byte[] GetImageBytes(ImageFormat format = ImageFormat.Png, int quality = 100)
    {
        SKEncodedImageFormat skFormat = format.ToSKFormat();

        if (format == ImageFormat.Bmp)
        {
            return GetBitmapBytes();
        }

        using SKData? skData = SKImage.Encode(skFormat, quality);

        return skData.ToArray();
    }

    public SavedImageInfo SaveJpeg(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Jpeg, quality);
        File.WriteAllBytes(path, bytes);

        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SavePng(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Png, 100);
        File.WriteAllBytes(path, bytes);

        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SaveBmp(string path)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Bmp, 100);
        File.WriteAllBytes(path, bytes);

        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo SaveWebp(string path, int quality = 85)
    {
        byte[] bytes = GetImageBytes(ImageFormat.Webp, quality);
        File.WriteAllBytes(path, bytes);

        return new SavedImageInfo(path, bytes.Length);
    }

    public SavedImageInfo Save(string path, ImageFormat format = ImageFormat.Png, int quality = 85)
    {
        return format switch
        {
            ImageFormat.Png => SavePng(path),
            ImageFormat.Jpeg => SaveJpeg(path, quality),
            ImageFormat.Webp => SaveWebp(path, quality),
            ImageFormat.Bmp => SaveBmp(path),
            ImageFormat.Svg => throw new ArgumentException($"Unsupported image format: {format}"),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        SKImage.Dispose();
        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    public void Render(SKCanvas canvas, PixelRect target, SKPaint paint, bool antiAlias)
    {
        paint.Color = SKColors.White;
        paint.FilterQuality = antiAlias ? SKFilterQuality.High : SKFilterQuality.None;
        canvas.DrawImage(SKImage, target.ToSKRect(), paint);
    }

    public byte[,] GetArrayGrayscale()
    {
        using SKBitmap bmp = SKBitmap.FromImage(SKImage);
        //int width = bmp.Width;
        //int height = bmp.Height;

        ReadOnlySpan<byte> spn = bmp.GetPixelSpan();
        byte[,] grayscaleBytes = new byte[bmp.Height, bmp.Width];

        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = ((y * bmp.Width) + x) * bmp.BytesPerPixel;
                byte r = spn[offset + 0];
                byte g = spn[offset + 1];
                byte b = spn[offset + 2];
                grayscaleBytes[y, x] = (byte)((r + g + b) / 3);
            }
        }

        return grayscaleBytes;
    }

    public byte[,,] GetArrayRGB()
    {
        using SKBitmap bmp = SKBitmap.FromImage(SKImage);
        //int width = bmp.Width;
        //int height = bmp.Height;

        ReadOnlySpan<byte> spn = bmp.GetPixelSpan();
        byte[,,] rgbBytes = new byte[bmp.Height, bmp.Width, 3];

        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = ((y * bmp.Width) + x) * bmp.BytesPerPixel;
                byte r = spn[offset + 0];
                byte g = spn[offset + 1];
                byte b = spn[offset + 2];
                rgbBytes[y, x, 2] = r;
                rgbBytes[y, x, 1] = g;
                rgbBytes[y, x, 0] = b;
            }
        }

        return rgbBytes;
    }

    public Image GetAutoscaledImage()
    {
        byte[,,] values = GetArrayRGB();

        byte maxValue = 0;

        for (int y = 0; y < values.GetLength(0); y++)
        {
            for (int x = 0; x < values.GetLength(1); x++)
            {
                for (int c = 0; c < values.GetLength(2); c++)
                {
                    maxValue = Math.Max(maxValue, values[y, x, c]);
                }
            }
        }

        for (int y = 0; y < values.GetLength(0); y++)
        {
            for (int x = 0; x < values.GetLength(1); x++)
            {
                for (int c = 0; c < values.GetLength(2); c++)
                {
                    values[y, x, c] = (byte)((double)values[y, x, c] / maxValue * 255);
                }
            }
        }

        return new Image(values);
    }
}
