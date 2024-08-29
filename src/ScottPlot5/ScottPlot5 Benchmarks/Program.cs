using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlotBench;

public static class Program
{
    private const string BenchmarkNamespace = $"{nameof(ScottPlotBench)}.{nameof(Benchmarks)}";

    public static void Main(string[] args)
    {
        Console.WriteLine("\nType 'f' to run a fast test configuration.");
        Console.WriteLine("Press ENTER to use the slower default configuration.");
        bool fast = Console.ReadKey().Key == ConsoleKey.F;
        string configType = fast ? "fast" : "default";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nProceeding with {configType} configuration...\n");
        IConfig config = fast ? Configurations.Fast : Configurations.Default;

        Type[] benchmarks = Assembly.GetExecutingAssembly().GetTypes().Where(static t => t.Namespace == BenchmarkNamespace).ToArray();

        BenchmarkSwitcher switcher = new BenchmarkSwitcher(benchmarks);
        switcher.Run(args, config);
    }
}
