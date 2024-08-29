namespace ScottPlot.DataGenerators;

public class RandomWalker2D(int? seed = null)
{
    private RandomDataGenerator Gen { get; } = new RandomDataGenerator(seed);

    private double _x;
    private double _y;
    private double _vx;
    private double _vy;

    public Coordinates Next()
    {
        _vx += Gen.RandomNumber() - .5;
        _vy += Gen.RandomNumber() - .5;

        double absX = Math.Abs(_vx);

        if (absX > 1)
        {
            _vx *= 1 - absX;
        }

        double absY = Math.Abs(_vy);

        if (absY > 1)
        {
            _vy *= 1 - absY;
        }

        _x += _vx;
        _y += _vy;

        return new Coordinates(_x, _y);
    }

    public IEnumerable<Coordinates> Next(int count) => Enumerable.Range(0, count).Select(_ => Next());
}
