namespace ScottPlotTests.RenderTests.Plottable;

internal class CandlestickTests
{
    [Test]
    public void TestCandlestickNegativeX()
    {
        Plot plot = new Plot();

        List<OHLC> price = [];

        DateTime startDate = new DateTime(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new OHLC(100, 103, 99, 102, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.ShiftedBy(i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.Candlestick(price);

        plot.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestOHLCNegativeX()
    {
        Plot plot = new Plot();

        List<OHLC> price = [];

        DateTime startDate = new DateTime(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new OHLC(100, 103, 99, 102, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.ShiftedBy(i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.OHLC(price);

        plot.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestCandlestickNoPriceChange()
    {
        Plot plot = new Plot();

        List<OHLC> price = [];

        DateTime startDate = new DateTime(1899, 12, 25); // NOTE: 1900 is the OADate rollover
        OHLC ohlc = new OHLC(100, 105, 95, 95, startDate, TimeSpan.FromDays(1));

        for (int i = 0; i < 10; i++)
        {
            price.Add(ohlc.WithClose(95 + i).ShiftedBy(TimeSpan.FromDays(i)));
        }

        plot.Add.Candlestick(price);

        plot.Should().SavePngWithoutThrowing();
    }
}
