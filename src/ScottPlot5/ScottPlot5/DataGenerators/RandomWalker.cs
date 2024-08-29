namespace ScottPlot.DataGenerators;

public class RandomWalker(int? seed = null, double firstValue = 0, double multiplier = 1)
{
    private readonly RandomDataGenerator _gen = new RandomDataGenerator(seed);
    private double _lastNumber = firstValue;

    public double Next() => _lastNumber += (_gen.RandomNumber() - .5) * multiplier;

    public IEnumerable<double> Next(int count) => Enumerable.Range(0, count).Select(_ => Next());
}
