using ScottPlot.DataSources;
using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.Palettes;

namespace ScottPlot;

/// <summary>
///     Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableAdder(Plot plot)
{
    public Plot Plot { get; } = plot;

    /// <summary>
    ///     Color set used for adding new plottables
    /// </summary>
    public IPalette Palette { get; set; } = new Category10();

    /// <summary>
    ///     Plottables of these types are ignored when assigning colors according to new plottables according to the
    ///     <see cref="Palette" />
    /// </summary>
    private readonly List<Type> _plottablesThatDoNotGetColors =
    [
        typeof(PolarAxis),
        typeof(Annotation),
        typeof(Benchmark),
        typeof(CandlestickPlot),
        typeof(OHLCPlot),
        typeof(Heatmap),
        typeof(ImageMarker),
        typeof(ImageRect),
        typeof(IsoLines),
        typeof(Pie),
        typeof(Text),
    ];

    private Color GetNextColor()
    {
        int coloredPlottableCount = Plot.PlottableList.Count(x => !_plottablesThatDoNotGetColors.Contains(x.GetType()));

        return Palette.GetColor(coloredPlottableCount);
    }

    public Annotation Annotation(string text, Alignment alignment = Alignment.UpperLeft)
    {
        Annotation an = new Annotation
        {
            Alignment = alignment,
            Text = text,
            LabelBackgroundColor = Colors.Yellow.WithAlpha(.75),
            LabelBorderColor = Colors.Black,
            LabelPadding = 5,
        };

        Plot.PlottableList.Add(an);

        return an;
    }

    public Arrow Arrow(Coordinates arrowBase, Coordinates arrowTip)
    {
        Color color = GetNextColor();

        Arrow arrow = new Arrow { Base = arrowBase, Tip = arrowTip, ArrowLineColor = color, ArrowFillColor = color.WithAlpha(.3), };

        Plot.PlottableList.Add(arrow);

        return arrow;
    }

    public Arrow Arrow(double xBase, double yBase, double xTip, double yTip)
    {
        Coordinates arrowBase = new Coordinates(xBase, yBase);
        Coordinates arrowTip = new Coordinates(xTip, yTip);

        return Arrow(arrowBase, arrowTip);
    }

    public Arrow Arrow(CoordinateLine line) => Arrow(line.Start, line.End);

    public BarPlot Bar(Bar bar)
    {
        BarPlot bp = new BarPlot(bar);
        Plottable(bp);

        return bp;
    }

    public BarPlot Bar(double position, double value, double error = 0)
    {
        Bar bar = new Bar { Position = position, Value = value, Error = error, FillColor = GetNextColor(), };

        return Bar(bar);
    }

    public BarPlot Bars(IList<Bar> bars)
    {
        BarPlot bp = new BarPlot(bars);
        Plottable(bp);

        return bp;
    }

    public BarPlot Bars(double[] values)
    {
        List<double> positions = Enumerable.Range(0, values.Length).Select(x => (double)x).ToList();

        return Bars(positions, values);
    }

    public BarPlot Bars(IList<double> positions, IList<double> values)
    {
        if (positions.Count != values.Count)
        {
            throw new ArgumentException($"{nameof(positions)} and {nameof(positions)} have different lengths");
        }

        Color color = GetNextColor();

        List<Bar> bars = positions.Zip(values, (a, b) => new { a, b }).Select(item => new Bar { Position = item.a, Value = item.b, FillColor = color }).ToList();

        return Bars(bars);
    }

    public BoxPlot Box(Box box)
    {
        BoxPlot bp = new BoxPlot();
        bp.Boxes.Add(box);
        bp.FillColor = GetNextColor();
        Plot.PlottableList.Add(bp);

        return bp;
    }

    public BoxPlot Boxes(IEnumerable<Box> boxes)
    {
        BoxPlot bp = new BoxPlot();
        bp.Boxes.AddRange(boxes);
        bp.FillColor = GetNextColor();
        Plot.PlottableList.Add(bp);

        return bp;
    }

    public Callout Callout(string text, double textX, double textY, double tipX, double tipY)
    {
        Coordinates labelCoordinates = new Coordinates(textX, textY);
        Coordinates lineCoordinates = new Coordinates(tipX, tipY);

        return Callout(text, labelCoordinates, lineCoordinates);
    }

    public Callout Callout(string text, Coordinates textLocation, Coordinates tipLocation)
    {
        Color color = GetNextColor();

        Callout callout = new Callout
        {
            Text = text,
            TextCoordinates = textLocation,
            TipCoordinates = tipLocation,
            ArrowLineColor = Colors.Transparent,
            ArrowFillColor = color,
            TextBackgroundColor = color.Lighten(.5),
            TextBorderColor = color,
            TextBorderWidth = 2,
            TextColor = Colors.Black,
            FontSize = 14,
        };

        Plot.PlottableList.Add(callout);

        return callout;
    }

    public CandlestickPlot Candlestick(List<OHLC> ohlcs)
    {
        OHLCSource dataSource = new OHLCSource(ohlcs);
        CandlestickPlot candlestickPlot = new CandlestickPlot(dataSource);
        Plot.PlottableList.Add(candlestickPlot);

        return candlestickPlot;
    }

    public Ellipse Circle(Coordinates center, double radius) => Ellipse(center, radius, radius);

    public Ellipse Circle(double xCenter, double yCenter, double radius) => Circle(new Coordinates(xCenter, yCenter), radius);

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new ColorBar(source, edge);

        Plot.Axes.Panels.Add(colorBar);

        return colorBar;
    }

    public Coxcomb Coxcomb(IList<PieSlice> slices)
    {
        Coxcomb coxcomb = new Coxcomb(slices);
        Plot.PlottableList.Add(coxcomb);

        return coxcomb;
    }

    public Coxcomb Coxcomb(IList<double> values)
    {
        List<PieSlice> slices = values.Select((_, i) => new PieSlice { Value = values[i], FillColor = Palette.GetColor(i).WithOpacity(0.5), }).ToList();

        Coxcomb coxcomb = new Coxcomb(slices);
        Plot.PlottableList.Add(coxcomb);

        return coxcomb;
    }

    public Crosshair Crosshair(double x, double y)
    {
        Crosshair ch = new Crosshair { Position = new Coordinates(x, y) };
        Color color = GetNextColor();
        ch.LineColor = color;
        ch.TextColor = color;
        Plot.PlottableList.Add(ch);

        return ch;
    }

    public DataLogger DataLogger()
    {
        DataLogger logger = new DataLogger { Color = GetNextColor(), };

        Plot.PlottableList.Add(logger);

        return logger;
    }

    public DataStreamer DataStreamer(int points, double period = 1)
    {
        double[] data = Generate.NaN(points);

        DataStreamer streamer = new DataStreamer(Plot, data) { Color = GetNextColor(), Period = period, };

        Plot.PlottableList.Add(streamer);

        return streamer;
    }

    public Ellipse Ellipse(Coordinates center, double radiusX, double radiusY, float rotation = 0)
    {
        Color color = GetNextColor();

        Ellipse ellipse = new Ellipse { Center = center, RadiusX = radiusX, RadiusY = radiusY, Rotation = rotation, LineColor = color, };

        Plot.PlottableList.Add(ellipse);

        return ellipse;
    }

    public Ellipse Ellipse(double xCenter, double yCenter, double radiusX, double radiusY, float rotation = 0)
        => Ellipse(new Coordinates(xCenter, yCenter), radiusX, radiusY, rotation);

    public ErrorBar ErrorBar(IReadOnlyList<double> xs, IReadOnlyList<double> ys, IReadOnlyList<double> yErrors)
    {
        ErrorBar eb = new ErrorBar(xs, ys, null, null, yErrors, yErrors) { Color = GetNextColor(), };

        Plot.PlottableList.Add(eb);

        return eb;
    }

    /// <summary>
    ///     Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(double[] xs, double[] ys1, double[] ys2)
    {
        return FillY(xs.Select((t, i) => (t, ys1[i], ys2[i])).ToList());
    }

    /// <summary>
    ///     Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(Scatter scatter1, Scatter scatter2)
    {
        FillY rangePlot = new FillY(scatter1, scatter2) { FillStyle = { Color = GetNextColor() } };
        Plot.PlottableList.Add(rangePlot);

        return rangePlot;
    }

    /// <summary>
    ///     Fill the vertical range between two Y points for each X point
    /// </summary>
    public FillY FillY(ICollection<(double X, double Top, double Bottom)> data)
    {
        FillY rangePlot = new FillY { FillStyle = { Color = GetNextColor() } };
        rangePlot.SetDataSource(data);
        Plot.PlottableList.Add(rangePlot);

        return rangePlot;
    }

    /// <summary>
    ///     Fill the vertical range between two Y points for each X point
    ///     This overload uses a custom function to calculate X, Y1, and Y2 values
    /// </summary>
    public FillY FillY<T>(ICollection<T> data, Func<T, (double X, double Top, double Bottom)> function)
    {
        FillY rangePlot = new FillY { FillStyle = { Color = GetNextColor() } };
        rangePlot.SetDataSource(data, function);
        Plot.PlottableList.Add(rangePlot);

        return rangePlot;
    }

    public FunctionPlot Function(IFunctionSource functionSource)
    {
        FunctionPlot functionPlot = new FunctionPlot(functionSource) { LineStyle = { Color = GetNextColor() } };

        Plot.PlottableList.Add(functionPlot);

        return functionPlot;
    }

    public FunctionPlot Function(Func<double, double> func)
    {
        FunctionSource functionSource = new FunctionSource(func);

        return Function(functionSource);
    }

    public Heatmap Heatmap(double[,] intensities)
    {
        Heatmap heatmap = new Heatmap(intensities);
        Plot.PlottableList.Add(heatmap);

        return heatmap;
    }

    public HorizontalLine HorizontalLine(double y, float width = 2, Color? color = null, LinePattern pattern = LinePattern.Solid)
    {
        Color color2 = color ?? GetNextColor();

        HorizontalLine line = new HorizontalLine
        {
            LineWidth = width,
            LineColor = color2,
            LabelBackgroundColor = color2,
            LabelFontColor = Colors.White,
            LinePattern = pattern,
            Y = y
        };

        Plot.PlottableList.Add(line);

        return line;
    }

    public HorizontalSpan HorizontalSpan(double x1, double x2, Color? color = null)
    {
        HorizontalSpan span = new HorizontalSpan { X1 = x1, X2 = x2, FillStyle = { Color = color ?? GetNextColor().WithAlpha(.2) } };
        span.LineStyle.Color = span.FillStyle.Color.WithAlpha(.5);
        Plot.PlottableList.Add(span);

        return span;
    }

    public ImageMarker ImageMarker(Coordinates location, Image image, float scale = 1)
    {
        ImageMarker marker = new ImageMarker { Location = location, Image = image, Scale = scale, };

        Plot.PlottableList.Add(marker);

        return marker;
    }

    public ImageRect ImageRect(Image image, CoordinateRect rect)
    {
        ImageRect marker = new ImageRect { Image = image, Rect = rect, };

        Plot.PlottableList.Add(marker);

        return marker;
    }

    public Legend Legend()
    {
        Legend legend = new Legend(Plot) { IsVisible = true };
        Plot.PlottableList.Add(legend);

        return legend;
    }

    public LinePlot Line(Coordinates start, Coordinates end)
    {
        LinePlot lp = new LinePlot { Start = start, End = end, LineStyle = { Color = GetNextColor() } };

        lp.MarkerStyle.FillColor = lp.LineStyle.Color;

        Plot.PlottableList.Add(lp);

        return lp;
    }

    public LinePlot Line(CoordinateLine line) => Line(line.Start, line.End);

    public LinePlot Line(double x1, double y1, double x2, double y2)
    {
        Coordinates start = new Coordinates(x1, y1);
        Coordinates end = new Coordinates(x2, y2);

        return Line(start, end);
    }

    public Marker Marker(double x, double y, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        Marker mp = new Marker { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor(), Location = new Coordinates(x, y), };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Marker Marker(Coordinates location, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
        => Marker(location.X, location.Y, shape, size, color);

    public Plottables.Markers Markers(double[] xs, double[] ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceDoubleArray ss = new ScatterSourceDoubleArray(xs, ys);

        Plottables.Markers mp = new Plottables.Markers(ss) { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(Coordinates[] coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesArray ss = new ScatterSourceCoordinatesArray(coordinates);

        Plottables.Markers mp = new Plottables.Markers(ss) { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers(List<Coordinates> coordinates, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceCoordinatesList ss = new ScatterSourceCoordinatesList(coordinates);

        Plottables.Markers mp = new Plottables.Markers(ss) { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(TX[] xs, TY[] ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericArray<TX, TY> ss = new ScatterSourceGenericArray<TX, TY>(xs, ys);

        Plottables.Markers mp = new Plottables.Markers(ss) { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public Plottables.Markers Markers<TX, TY>(List<TX> xs, List<TY> ys, MarkerShape shape = MarkerShape.FilledCircle, float size = 10, Color? color = null)
    {
        ScatterSourceGenericList<TX, TY> ss = new ScatterSourceGenericList<TX, TY>(xs, ys);

        Plottables.Markers mp = new Plottables.Markers(ss) { MarkerShape = shape, MarkerSize = size, Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(mp);

        return mp;
    }

    public OHLCPlot OHLC(List<OHLC> ohlcs)
    {
        OHLCSource dataSource = new OHLCSource(ohlcs);
        OHLCPlot ohlc = new OHLCPlot(dataSource);
        Plot.PlottableList.Add(ohlc);

        return ohlc;
    }

    public Phasor Phasor()
    {
        Phasor phasor = new Phasor();

        Color color = GetNextColor().WithAlpha(0.7);
        phasor.ArrowFillColor = color;
        phasor.ArrowLineColor = color;
        phasor.LabelStyle.ForeColor = phasor.ArrowFillColor;

        Plot.PlottableList.Add(phasor);

        return phasor;
    }

    public Phasor Phasor(IEnumerable<PolarCoordinates> points)
    {
        Phasor phasor = Phasor();
        phasor.Points.AddRange(points);

        return phasor;
    }

    public Pie Pie(IList<PieSlice> slices)
    {
        Pie pie = new Pie(slices);
        Plot.PlottableList.Add(pie);

        return pie;
    }

    public Pie Pie(IList<double> values)
    {
        List<PieSlice> slices = values.Select((_, i) => new PieSlice { Value = values[i], FillColor = Palette.GetColor(i), }).ToList();

        Pie pie = new Pie(slices);
        Plot.PlottableList.Add(pie);

        return pie;
    }

    public IPlottable Plottable(IPlottable plottable)
    {
        Plot.PlottableList.Add(plottable);

        return plottable;
    }

    public PolarAxis PolarAxis(double maximumRadius = 1, bool hideCartesianAxesAndGrids = true)
    {
        PolarAxis pol = new PolarAxis { MaximumRadius = maximumRadius, };

        pol.RegenerateCircles();
        pol.RegenerateSpokes();

        Plot.PlottableList.Add(pol);

        if (hideCartesianAxesAndGrids)
        {
            Plot.HideAxesAndGrid();
        }

        return pol;
    }

    public Polygon Polygon(Coordinates[] coordinates)
    {
        Color color = GetNextColor();
        Polygon poly = new Polygon(coordinates) { LineColor = color, FillColor = color.WithAlpha(.5), };
        Plot.PlottableList.Add(poly);

        return poly;
    }

    public Polygon Polygon<TX, TY>(IEnumerable<TX> xs, IEnumerable<TY> ys)
    {
        Coordinates[] coordinates = NumericConversion.GenericToCoordinates(xs, ys);

        return Polygon(coordinates);
    }

    public PopulationSymbol Population(double[] values, double x = 0)
    {
        Color color = GetNextColor();
        Population pop = new Population(values);

        PopulationSymbol sym = new PopulationSymbol(pop)
        {
            X = x,
            Bar = { FillColor = color },
            Box = { FillColor = Colors.Black.WithLightness(.8f) },
            Marker = { MarkerLineColor = color, MarkerFillColor = color }
        };

        Plot.PlottableList.Add(sym);

        return sym;
    }

    public Radar Radar(IReadOnlyList<RadarSeries> series)
    {
        Radar radar = new Radar(series);
        Plot.PlottableList.Add(radar);

        return radar;
    }

    public Radar Radar(IEnumerable<IEnumerable<double>> series)
    {
        List<RadarSeries> radarSeries = [];

        foreach (IEnumerable<double> values in series)
        {
            RadarSeries radarSerie = new RadarSeries(values.ToList(), Palette.GetColor(radarSeries.Count).WithOpacity(0.5));
            radarSeries.Add(radarSerie);
        }

        Radar radar = new Radar(radarSeries);
        Plot.PlottableList.Add(radar);

        return radar;
    }

    public RadialGaugePlot RadialGaugePlot(IList<double> values)
    {
        Color[] colors = Enumerable.Range(0, values.Count).Select(Palette.GetColor).ToArray();
        RadialGaugePlot radialGaugePlot = new RadialGaugePlot([.. values], colors);
        Plot.PlottableList.Add(radialGaugePlot);
        Plot.HideGrid();
        Plot.Layout.Frameless();

        return radialGaugePlot;
    }

    public Rectangle Rectangle(CoordinateRect rect) => Rectangle(rect.Left, rect.Right, rect.Top, rect.Bottom);

    public Rectangle Rectangle(double left, double right, double bottom, double top)
    {
        Color color = GetNextColor();

        Rectangle rp = new Rectangle
        {
            X1 = left,
            X2 = right,
            Y1 = bottom,
            Y2 = top,
            LineColor = color,
            FillColor = color.WithAlpha(.5),
        };

        Plot.PlottableList.Add(rp);

        return rp;
    }

    public Scatter Scatter(IScatterSource source, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        Scatter scatter = new Scatter(source) { LineColor = nextColor, MarkerFillColor = nextColor, MarkerLineColor = nextColor, };
        Plot.PlottableList.Add(scatter);

        return scatter;
    }

    public Scatter Scatter(double x, double y, Color? color = null)
    {
        double[] xs = [x];
        double[] ys = [y];
        ScatterSourceDoubleArray source = new ScatterSourceDoubleArray(xs, ys);

        return Scatter(source, color);
    }

    public Scatter Scatter(double[] xs, double[] ys, Color? color = null)
    {
        ScatterSourceDoubleArray source = new ScatterSourceDoubleArray(xs, ys);

        return Scatter(source, color);
    }

    public Scatter Scatter(Coordinates point, Color? color = null)
    {
        Coordinates[] coordinates = [point];
        ScatterSourceCoordinatesArray source = new ScatterSourceCoordinatesArray(coordinates);

        return Scatter(source, color);
    }

    public Scatter Scatter(Coordinates[] coordinates, Color? color = null)
    {
        ScatterSourceCoordinatesArray source = new ScatterSourceCoordinatesArray(coordinates);

        return Scatter(source, color);
    }

    public Scatter Scatter(List<Coordinates> coordinates, Color? color = null)
    {
        ScatterSourceCoordinatesList source = new ScatterSourceCoordinatesList(coordinates);

        return Scatter(source, color);
    }

    public Scatter Scatter<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        ScatterSourceGenericArray<T1, T2> source = new ScatterSourceGenericArray<T1, T2>(xs, ys);
        Scatter scatter = new Scatter(source) { LineStyle = { Color = nextColor }, MarkerStyle = { FillColor = nextColor } };
        Plot.PlottableList.Add(scatter);

        return scatter;
    }

    public Scatter Scatter<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        Color nextColor = color ?? GetNextColor();
        ScatterSourceGenericList<T1, T2> source = new ScatterSourceGenericList<T1, T2>(xs, ys);
        Scatter scatter = new Scatter(source) { LineStyle = { Color = nextColor }, MarkerStyle = { FillColor = nextColor } };
        Plot.PlottableList.Add(scatter);

        return scatter;
    }

    public Scatter ScatterLine(IScatterSource source, Color? color = null)
    {
        Scatter scatter = Scatter(source, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterLine(double[] xs, double[] ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterLine(Coordinates[] coordinates, Color? color = null)
    {
        Scatter scatter = Scatter(coordinates, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterLine(List<Coordinates> coordinates, Color? color = null)
    {
        Scatter scatter = Scatter(coordinates, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterLine<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterLine<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.MarkerSize = 0;

        return scatter;
    }

    public Scatter ScatterPoints(IScatterSource source, Color? color = null)
    {
        Scatter scatter = Scatter(source, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Scatter ScatterPoints(double[] xs, double[] ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Scatter ScatterPoints(Coordinates[] coordinates, Color? color = null)
    {
        Scatter scatter = Scatter(coordinates, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Scatter ScatterPoints(List<Coordinates> coordinates, Color? color = null)
    {
        Scatter scatter = Scatter(coordinates, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Scatter ScatterPoints<T1, T2>(T1[] xs, T2[] ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Scatter ScatterPoints<T1, T2>(List<T1> xs, List<T2> ys, Color? color = null)
    {
        Scatter scatter = Scatter(xs, ys, color);
        scatter.LineWidth = 0;

        return scatter;
    }

    public Signal Signal(ISignalSource source, Color? color = null)
    {
        Signal sig = new Signal(source) { Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public Signal Signal(double[] ys, double period = 1, Color? color = null)
    {
        SignalSourceDouble source = new SignalSourceDouble(ys, period);

        return Signal(source, color);
    }

    public Signal Signal<T>(T[] ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericArray<T> source = new SignalSourceGenericArray<T>(ys, period);

        return Signal(source, color);
    }

    public Signal Signal<T>(IReadOnlyList<T> ys, double period = 1, Color? color = null)
    {
        SignalSourceGenericList<T> source = new SignalSourceGenericList<T>(ys, period);

        return Signal(source, color);
    }

    public SignalConst<T> SignalConst<T>(T[] ys, double period = 1, Color? color = null)
        where T : struct, IComparable
    {
        SignalConst<T> sig = new SignalConst<T>(ys, period) { Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalXY SignalXY(ISignalXYSource source, Color? color = null)
    {
        SignalXY sig = new SignalXY(source) { Color = color ?? GetNextColor() };

        Plot.PlottableList.Add(sig);

        return sig;
    }

    public SignalXY SignalXY(double[] xs, double[] ys, Color? color = null)
    {
        SignalXYSourceDoubleArray source = new SignalXYSourceDoubleArray(xs, ys);

        return SignalXY(source, color);
    }

    public SignalXY SignalXY<TX, TY>(TX[] xs, TY[] ys, Color? color = null)
    {
        SignalXYSourceGenericArray<TX, TY> source = new SignalXYSourceGenericArray<TX, TY>(xs, ys);

        return SignalXY(source, color);
    }

    public Text Text(string text, Coordinates location) => Text(text, location.X, location.Y);

    public Text Text(string? text, double x, double y)
    {
        Text txt = new Text
        {
            LabelText = text ?? string.Empty,
            LabelBackgroundColor = Colors.Transparent,
            LabelBorderColor = Colors.Transparent,
            Location = new Coordinates(x, y)
        };

        Plot.PlottableList.Add(txt);

        return txt;
    }

    public VectorField VectorField(IList<RootedCoordinateVector> vectors, Color? color = null)
    {
        VectorFieldDataSourceCoordinatesList vs = new VectorFieldDataSourceCoordinatesList(vectors);
        VectorField field = new VectorField(vs);
        field.ArrowStyle.LineStyle.Color = color ?? GetNextColor();
        field.ArrowStyle.LineStyle.Width = 2;
        Plot.PlottableList.Add(field);

        return field;
    }

    public VerticalLine VerticalLine(double x, float width = 2, Color? color = null, LinePattern pattern = LinePattern.Solid)
    {
        Color color2 = color ?? GetNextColor();
        VerticalLine line = new VerticalLine { LineWidth = width, LineColor = color2, LabelBackgroundColor = color2, LinePattern = pattern, X = x };
        Plot.PlottableList.Add(line);

        return line;
    }

    public VerticalSpan VerticalSpan(double y1, double y2, Color? color = null)
    {
        VerticalSpan span = new VerticalSpan { Y1 = y1, Y2 = y2, FillStyle = { Color = color ?? GetNextColor().WithAlpha(.2) } };
        span.LineStyle.Color = span.FillStyle.Color.WithAlpha(.5);
        Plot.PlottableList.Add(span);

        return span;
    }
}
