using ScottPlot.Colormaps;

namespace ScottPlot.Plottables;

public class Heatmap(double[,] intensities) : IPlottable, IHasColorAxis
{
    /// <summary>
    ///     Data values for the heatmap.
    ///     <see cref="Update" /> must be called after changing this array or editing its values.
    /// </summary>
    public double[,] Intensities { get; set; } = intensities;

    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    private IColormap _colormap = new Viridis();

    public IColormap Colormap
    {
        get => _colormap;
        set
        {
            _colormap = value;
            Update();
        }
    }

    /// <summary>
    ///     Indicates position of the data point relative to the rectangle used to represent it.
    ///     An alignment of upper right means the rectangle will appear to the lower left of the point itself.
    /// </summary>
    private Alignment _cellAlignment = Alignment.MiddleCenter;

    public Alignment CellAlignment
    {
        get => _cellAlignment;
        set
        {
            _cellAlignment = value;
            Update();
        }
    }

    /// <summary>
    ///     If defined, the this rectangle sets the axis boundaries of heatmap data.
    ///     Note that the actual heatmap area is 1 cell larger than this rectangle.
    /// </summary>
    private CoordinateRect? _extent;

    public CoordinateRect? Extent // TODO: obsolete this
    {
        get => _extent;
        set
        {
            _extent = value;
            Update();
        }
    }

    /// <summary>
    ///     If defined, the this rectangle sets the axis boundaries of heatmap data.
    ///     Note that the actual heatmap area is 1 cell larger than this rectangle.
    /// </summary>
    public CoordinateRect? Position { get => Extent; set => Extent = value; }

    /// <summary>
    ///     This variable controls whether row 0 of the 2D source array is the top or bottom of the heatmap.
    ///     When set to false (default), row 0 is the top of the heatmap.
    ///     When set to true, row 0 of the source data will be displayed at the bottom.
    /// </summary>
    private bool _flipRows;

    public bool FlipVertically
    {
        get => _flipRows;
        set
        {
            _flipRows = value;
            Update();
        }
    }

    /// <summary>
    ///     This variable controls whether the first sample in each column of the 2D source array is the left or right of the
    ///     heatmap.
    ///     When set to false (default), sample 0 is the left of the heatmap.
    ///     When set to true, sample 0 of the source data will be displayed at the right.
    /// </summary>
    private bool _flipColumns;

    public bool FlipHorizontally
    {
        get => _flipColumns;
        set
        {
            _flipColumns = value;
            Update();
        }
    }

    public bool FlipColumns { get => FlipHorizontally; set => FlipHorizontally = value; }

    public bool FlipRows { get => FlipVertically; set => FlipVertically = value; }

    /// <summary>
    ///     If true, pixels in the final image will be interpolated to give the heatmap a smooth appearance.
    ///     If false, the heatmap will appear as individual rectangles with sharp edges.
    /// </summary>
    private bool _smooth;

    public bool Smooth
    {
        get => _smooth;
        set
        {
            _smooth = value;
            Update();
        }
    }

    /// <summary>
    ///     Actual extent of the heatmap bitmap after alignment has been applied
    /// </summary>
    private CoordinateRect AlignedExtent
    {
        get
        {
            double xOffset = Math.Abs(CellWidth) * CellAlignment.HorizontalFraction();
            double yOffset = Math.Abs(CellHeight) * CellAlignment.VerticalFraction();
            Coordinates cellOffset = new Coordinates(-xOffset, -yOffset);

            return ExtentOrDefault.WithTranslation(cellOffset);
        }
    }

    /// <summary>
    ///     Extent used at render time.
    ///     Supplies the user-provided extent if available,
    ///     otherwise a heatmap centered at the origin with cell size 1.
    /// </summary>
    private CoordinateRect ExtentOrDefault
    {
        get
        {
            if (Extent is not CoordinateRect extent)
            {
                return new CoordinateRect(0, Intensities.GetLength(1), 0, Intensities.GetLength(0));
            }

            //CoordinateRect extent = Extent.Value;
            //user will provide the extends to the data. The image will be one cell wider and taller so we need to add that on (it is being added on in teh default case).
            double cellwidth = extent.Width / (Intensities.GetLength(1) - 1);
            double cellheight = extent.Height / (Intensities.GetLength(0) - 1);

            if (extent.Left < extent.Right)
            {
                extent.Right += cellwidth;
            }

            if (extent.Left > extent.Right)
            {
                extent.Left -= cellwidth; //cellwidth will be negative if extent is flipped
            }

            if (extent.Bottom < extent.Top)
            {
                extent.Top += cellheight;
            }

            if (extent.Bottom > extent.Top)
            {
                extent.Bottom -= cellheight; //cellheight will be negative if extent is inverted
            }

            return extent;
        }
    }

    /// <summary>
    ///     Width of a single cell from the heatmap (in coordinate units)
    /// </summary>
    public double CellWidth
    {
        get => ExtentOrDefault.Width / Intensities.GetLength(1);
        set
        {
            double left = ExtentOrDefault.Left;
            double right = ExtentOrDefault.Left + (value * Intensities.GetLength(1));
            double bottom = ExtentOrDefault.Bottom;
            double top = ExtentOrDefault.Top;
            Extent = new CoordinateRect(left, right, bottom, top);
        }
    }

    /// <summary>
    ///     Height of a single cell from the heatmap (in coordinate units)
    /// </summary>
    public double CellHeight
    {
        get => ExtentOrDefault.Height / Intensities.GetLength(0);
        set
        {
            double left = ExtentOrDefault.Left;
            double right = ExtentOrDefault.Right;
            double bottom = ExtentOrDefault.Bottom;
            double top = ExtentOrDefault.Bottom + (value * Intensities.GetLength(0));
            Extent = new CoordinateRect(left, right, bottom, top);
        }
    }

    /// <summary>
    ///     Defines what color will be used to fill cells containing NaN.
    /// </summary>
    public Color NaNCellColor
    {
        get => _naNCellColor;
        set
        {
            _naNCellColor = value;
            Update();
        }
    }

    private Color _naNCellColor = Colors.Transparent;

    private byte[,]? _alphaMap;

    /// <summary>
    ///     If present, this array defines transparency for each cell in the heatmap.
    ///     Values range from 0 (transparent) through 255 (opaque).
    /// </summary>
    public byte[,]? AlphaMap
    {
        get => _alphaMap;
        set
        {
            if (value?.GetLength(0) != Height)
            {
                throw new Exception("AlphaMap height must match the height of the Intensity map.");
            }

            if (value.GetLength(1) != Width)
            {
                throw new Exception("AlphaMap width must match the width of the Intensity map.");
            }

            _alphaMap = value;

            Update();
        }
    }

    private double _opacity = 1;

    /// <summary>
    ///     Controls the opacity of the entire heatmap from 0 (transparent) to 1 (opaque)
    /// </summary>
    public double Opacity
    {
        get => _opacity;
        set
        {
            _opacity = NumericConversion.Clamp(value, 0, 1);
            Update();
        }
    }

    /// <summary>
    ///     Height of the heatmap data (rows)
    /// </summary>
    private int Height => Intensities.GetLength(0);

    /// <summary>
    ///     Width of the heatmap data (columns)
    /// </summary>
    private int Width => Intensities.GetLength(1);

    /// <summary>
    ///     Generated and stored when <see cref="Update" /> is called
    /// </summary>
    private SKBitmap? _bitmap;

    ~Heatmap()
    {
        _bitmap?.Dispose();
    }

    /// <summary>
    ///     Return heatmap as an array of ARGB values,
    ///     scaled according to the heatmap setting,
    ///     and in the order necessary to create a bitmap.
    /// </summary>
    private uint[] GetArgbValues()
    {
        Range range = GetRange();
        uint[] argb = new uint[Intensities.Length];

        // the XOR here disables flipping when the flip property and the extent is inverted.
        bool flipY = FlipVertically ^ ExtentOrDefault.IsInvertedY;
        bool flipX = FlipHorizontally ^ ExtentOrDefault.IsInvertedX;

        uint nanCellArgb = NaNCellColor.PremultipliedARGB;

        for (int y = 0; y < Height; y++)
        {
            int rowOffset = flipY ? (Height - 1 - y) * Width : y * Width;

            for (int x = 0; x < Width; x++)
            {
                int xIndex = flipX ? Width - 1 - x : x;

                // Make NaN cells transparent
                if (double.IsNaN(Intensities[y, xIndex]))
                {
                    argb[rowOffset + x] = nanCellArgb;

                    continue;
                }

                Color cellColor = Colormap.GetColor(Intensities[y, xIndex], range);

                if (AlphaMap is not null)
                {
                    cellColor = cellColor.WithAlpha(AlphaMap[y, xIndex]);
                }

                if (Opacity != 1)
                {
                    cellColor = cellColor.WithAlpha((byte)(cellColor.Alpha * Opacity));
                }

                argb[rowOffset + x] = cellColor.PremultipliedARGB;
            }
        }

        return argb;
    }

    /// <summary>
    ///     Regenerate the image using the present settings and data in <see cref="Intensities" />
    /// </summary>
    public void Update()
    {
        DataRange = Range.GetRange(Intensities);
        uint[] argbs = GetArgbValues();
        _bitmap?.Dispose();
        _bitmap = Drawing.BitmapFromArgbs(argbs, Width, Height);
    }

    public AxisLimits GetAxisLimits() => new AxisLimits(AlignedExtent);

    /// <summary>
    ///     Return the position in the array beneath the given point
    /// </summary>
    public (int x, int y) GetIndexes(Coordinates coordinates)
    {
        CoordinateRect rect = AlignedExtent;

        double distanceFromLeft = coordinates.X - rect.Left;
        int xIndex = (int)(distanceFromLeft / CellWidth);

        double distanceFromTop = rect.Top - coordinates.Y;
        int yIndex = (int)(distanceFromTop / CellHeight);

        return (xIndex, yIndex);
    }

    /// <summary>
    ///     Return the value of the cell beneath the given point.
    ///     Returns NaN if the point is outside the heatmap area.
    /// </summary>
    public double GetValue(Coordinates coordinates)
    {
        CoordinateRect rect = AlignedExtent;

        if (!rect.Contains(coordinates))
        {
            return double.NaN;
        }

        (int xIndex, int yIndex) = GetIndexes(coordinates);

        return Intensities[yIndex, xIndex];
    }

    public IEnumerable<LegendItem> LegendItems => [];

    public Range GetRange() => ManualRange ?? DataRange;

    /// <summary>
    ///     Range of values spanned by the data the last time it was updated
    /// </summary>
    public Range DataRange { get; private set; }

    private Range? _manualRange;

    /// <summary>
    ///     If supplied, the colormap will span this range of values
    /// </summary>
    public Range? ManualRange
    {
        get => _manualRange;
        set
        {
            _manualRange = value;
            Update();
        }
    }

    public virtual void Render(RenderPack rp)
    {
        if (_bitmap is null)
        {
            Update(); // automatically generate the bitmap on first render if it was not generated manually
        }

        using SKPaint paint = new SKPaint();
        paint.FilterQuality = Smooth ? SKFilterQuality.High : SKFilterQuality.None;

        SKRect rect = Axes.GetPixelRect(AlignedExtent).ToSKRect();

        rp.Canvas.DrawBitmap(_bitmap, rect, paint);
    }
}
