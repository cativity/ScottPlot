﻿using JetBrains.Annotations;
using ScottPlot;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class LiveFinance : Form, IDemoWindow
{
    public string Title => "Live Financial Plot";

    public string Description
        => "Demonstrates how to display price OHLC data in real time, modifying the last bar for live updates and adding new bars as time progresses.";

    private readonly List<OHLC> _ohlCs;

    private readonly Timer _timer;

    public LiveFinance()
    {
        InitializeComponent();

        // create past OHLC data
        _ohlCs = [];
        const int historicalPointCount = 10;
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = TimeSpan.FromSeconds(5);
        DateTime oldest = now - (historicalPointCount * timeSpan);

        for (int i = 0; i < historicalPointCount; i++)
        {
            DateTime start = oldest + (timeSpan * i);
            OHLC ohlc = GetRandomOhlc(start, timeSpan);
            _ohlCs.Add(ohlc);
        }

        // plot the OHLC list
        formsPlot1.Plot.Add.Candlestick(_ohlCs);

        // setup the plot to display X axis tick labels using date time format
        formsPlot1.Plot.Axes.DateTimeTicksBottom();

        // setup a timer to update the chart every second
        _timer = new Timer { Interval = 1000, Enabled = true };
        _timer.Tick += TimerTick;
    }

    private static OHLC GetRandomOhlc(DateTime dt, TimeSpan ts)
    {
        double open = Generate.RandomNumber(95, 105);
        double close = Generate.RandomNumber(95, 105);
        double low = 95 - Generate.RandomNumber(5);
        double high = 105 + Generate.RandomNumber(5);

        return new OHLC(open, high, low, close, dt, ts);
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        DateTime now = DateTime.Now;

        if (now.Second % 5 == 0)
        {
            // start a new OHLC every 5 seconds
            OHLC newPrice = GetRandomOhlc(DateTime.Now, TimeSpan.FromSeconds(5));
            _ohlCs.Add(newPrice);
        }
        else
        {
            // modify the last OHLC by increasing its closing price
            int lastOhlcIndex = _ohlCs.Count - 1;
            OHLC updatedOhlc = _ohlCs[lastOhlcIndex];
            updatedOhlc.Close += .5;
            _ohlCs[lastOhlcIndex] = updatedOhlc;
        }

        formsPlot1.Plot.Axes.AutoScaleExpand();
        formsPlot1.Refresh();
    }
}
