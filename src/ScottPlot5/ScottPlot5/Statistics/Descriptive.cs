namespace ScottPlot.Statistics;

public static class Descriptive
{
    /* NOTES:
     *   The core functions in this class should support double[].
     *   Overloads may support <T> but they just convert to double[] then call the other function.
     *   IReadOnlyList is favored to IEnumerable to avoid Count().
     *   Code here may benefit from benchmarking to confirm which strategies are most performant.
     *   Additional discussion: https://github.com/ScottPlot/ScottPlot/pull/3071.
     */

    /// <summary>
    ///     Return the sample sum.
    /// </summary>
    public static double Sum(double[] values)
    {
        return values.Length > 0 ? values.Sum() : throw new ArgumentException($"{nameof(values)} cannot be empty");
    }

    /// <summary>
    ///     Return the sample sum.
    /// </summary>
    public static double Sum<T>(IReadOnlyList<T> values)
    {
        return Sum(NumericConversion.GenericToDoubleArray(values));
    }

    /// <summary>
    ///     Return the sample mean.
    /// </summary>
    public static double Mean(double[] values)
    {
        return values.Length > 0 ? Sum(values) / values.Length : throw new ArgumentException($"{nameof(values)} cannot be empty");
    }

    /// <summary>
    ///     Return the sample median.
    /// </summary>
    public static double Median(double[] values)
    {
        return values.Length > 0
                   ? SortedMedian((double[]) [.. values.OrderBy(static x => x)])
                   : throw new ArgumentException($"{nameof(values)} cannot be empty");
    }

    /// <summary>
    ///     Return the median of a sorted sample.
    /// </summary>
    public static double SortedMedian(IReadOnlyList<double> sortedValues)
    {
        if (sortedValues.Count % 2 == 1)
        {
            return sortedValues[sortedValues.Count / 2];
        }

        double left = sortedValues[(sortedValues.Count / 2) - 1];
        double right = sortedValues[sortedValues.Count / 2];

        return (left + right) / 2;
    }

    /// <summary>
    ///     Return the percentile of a sorted sample.
    /// </summary>
    public static double SortedPercentile(IReadOnlyList<double> sortedValues, double percentile)
    {
        int index = (int)(percentile * sortedValues.Count / 100);
        index = Math.Max(0, index);
        index = Math.Min(sortedValues.Count - 1, index);

        return sortedValues[index];
    }

    /// <summary>
    ///     Return the percentile of a sample.
    /// </summary>
    public static double Percentile(IReadOnlyList<double> values, double percentile)
    {
        return SortedPercentile([.. values.OrderBy(static x => x)], percentile);
    }

    /// <summary>
    ///     Return the sample mean.
    /// </summary>
    public static double Mean<T>(IEnumerable<T> values)
    {
        return Mean(NumericConversion.GenericToDoubleArray(values));
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values.
    ///     Use this function when your data is a sample from a population.
    ///     To calculate the variance from the entire population use <see cref="VarianceP(double[])" />.
    /// </summary>
    public static double Variance(double[] values)
    {
        if (values.Length == 0)
        {
            throw new ArgumentException($"{nameof(values)} must not be empty");
        }

        if (values.Length == 1)
        {
            return 0;
        }

        // TODO: benchmark and see if this can be optimized by not using LINQ
        double mean = Mean(values);
        double variance = values.Select(x => x - mean).Sum(x => x * x);

        return variance / (values.Length - 1);
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values.
    ///     Use this function to calculate the variance from the entire population.
    ///     To estimate the variance from a sample, use <see cref="VarianceP{T}(IReadOnlyList{T})()" />.
    /// </summary>
    public static double Variance<T>(IReadOnlyList<T> values)
    {
        return Variance(NumericConversion.GenericToDoubleArray(values));
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values.
    ///     Use this function to calculate the variance from the entire population.
    ///     To estimate the variance from a sample, use <see cref="Variance(double[])" />.
    /// </summary>
    public static double VarianceP(double[] values)
    {
        if (values.Length == 0)
        {
            throw new ArgumentException($"{nameof(values)} must not be empty");
        }

        if (values.Length == 1)
        {
            return 0;
        }

        // TODO: benchmark and see if this can be optimized by not using LINQ
        double mean = Mean(values);
        double variance = values.Select(x => x - mean).Sum(x => x * x);

        return variance / values.Length;
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values.
    ///     Use this function to calculate the variance from the entire population.
    ///     To estimate the variance from a sample, use <see cref="Variance{T}(IReadOnlyList{T})" />.
    /// </summary>
    public static double VarianceP<T>(IReadOnlyList<T> values)
    {
        return VarianceP(NumericConversion.GenericToDoubleArray(values));
    }

    /// <summary>
    ///     Return the sample standard deviation (the square root of the sample variance).
    /// </summary>
    public static double StandardDeviation(double[] values) => Math.Sqrt(Variance(values));

    /// <summary>
    ///     Return the sample standard deviation (the square root of the sample variance).
    /// </summary>
    public static double StandardDeviation<T>(IReadOnlyList<T> values)
    {
        return values.Count > 0
                   ? StandardDeviation(NumericConversion.GenericToDoubleArray(values))
                   : throw new ArgumentException($"{nameof(values)} cannot be empty");
    }

    /// <summary>
    ///     Return the population standard deviation (the square root of the population variance).
    ///     See VarianceP() for more information.
    /// </summary>
    public static double StandardDeviationP(double[] values) => Math.Sqrt(VarianceP(values));

    /// <summary>
    ///     Return the population standard deviation (the square root of the population variance).
    ///     See VarianceP() for more information.
    /// </summary>
    public static double StandardDeviationP<T>(IReadOnlyList<T> values)
    {
        return values.Count > 0
                   ? StandardDeviationP(NumericConversion.GenericToDoubleArray(values))
                   : throw new ArgumentException($"{nameof(values)} cannot be empty");
    }

    /// <summary>
    ///     Standard error of the mean.
    /// </summary>
    public static double StandardError<T>(IReadOnlyList<T> values) => StandardDeviation(values) / Math.Sqrt(values.Count);

    public static IReadOnlyList<double> RemoveNaN<T>(IReadOnlyList<T> values)
    {
        return NumericConversion.GenericToDoubleArray(values).Where(static value => !double.IsNaN(value)).ToList();
    }

    public static double[] RemoveNaN(double[] values)
    {
        return values.Where(static value => !double.IsNaN(value)).ToArray();
    }

    /// <summary>
    ///     Return the sample mean. NaN values are ignored.
    ///     Returns NaN if all values are NaN.
    /// </summary>
    public static double NanMean<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Any() ? Mean(real) : double.NaN;
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values. NaN values are ignored.
    ///     Use this function when your data is a sample from a population.
    ///     To calculate the variance from the entire population use <see cref="VarianceP(double[])" />.
    /// </summary>
    public static double NanVariance<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Count > 1 ? Variance(real) : double.NaN;
    }

    /// <summary>
    ///     Return the sample variance (second moment about the mean) of data.
    ///     Input must contain at least two values. NaN values are ignored.
    ///     Use this function when your data is a sample from a population.
    ///     To calculate the variance from the entire population use <see cref="VarianceP(double[])" />.
    /// </summary>
    public static double NanVarianceP<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Count > 1 ? VarianceP(real) : double.NaN;
    }

    /// <summary>
    ///     Return the sample standard deviation (the square root of the sample variance).
    ///     NaN values are ignored.
    /// </summary>
    public static double NanStandardDeviation<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Count > 1 ? StandardDeviation(real) : double.NaN;
    }

    /// <summary>
    ///     Return the population standard deviation (the square root of the sample variance).
    ///     NaN values are ignored.
    /// </summary>
    public static double NanStandardDeviationP<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Count > 1 ? StandardDeviationP(real) : double.NaN;
    }

    /// <summary>
    ///     Standard error of the mean.
    ///     NaN values are ignored.
    /// </summary>
    public static double NanStandardError<T>(IReadOnlyList<T> values)
    {
        IReadOnlyList<double> real = RemoveNaN(values);

        return real.Count > 0 ? StandardError(real) : double.NaN;
    }

    /// <summary>
    ///     Transpose a multidimensional (not jagged) array
    /// </summary>
    public static double[,] ArrayTranspose(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        double[,] result = new double[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    /// <summary>
    ///     Extracts a row or column from a 2D array
    /// </summary>
    public static double[] ArrayToVector(double[,] values, uint? row = 0, uint? column = null)
    {
        if (values.GetLength(0) == 0)
        {
            throw new ArgumentException($"Array {nameof(values)} cannot be empty");
        }

        double[] vector = [];

        if (row is not null)
        {
            vector = new double[values.GetLength(1)];
            Array.Copy(values, (int)row * values.GetLength(1), vector, 0, values.GetLength(1));
        }
        else if (column is not null)
        {
            vector = ArrayToVector(ArrayTranspose(values), column);
        }

        return vector;
    }

    public static double NanMean(double[,] values, uint row = 0, uint? column = null)
    {
        return NanMean(ArrayToVector(values, row, column));
    }

    public static double NanVariance(double[,] values, uint row = 0, uint? column = null)
    {
        return Variance(RemoveNaN(ArrayToVector(values, row, column)));
    }

    public static double NanVarianceP(double[,] values, uint row = 0, uint? column = null)
    {
        return VarianceP(RemoveNaN(ArrayToVector(values, row, column)));
    }

    public static double NanStandardDeviation(double[,] values, uint row = 0, uint? column = null)
    {
        return StandardDeviation(RemoveNaN(ArrayToVector(values, row, column)));
    }

    public static double NanStandardDeviationP(double[,] values, uint row = 0, uint? column = null)
    {
        return StandardDeviationP(RemoveNaN(ArrayToVector(values, row, column)));
    }

    public static double NanStandardError(double[,] values, uint row = 0, uint? column = null)
    {
        return NanStandardError(RemoveNaN(ArrayToVector(values, row, column)));
    }

    public static double[] VerticalSlice(double[,] values, int columnIndex)
    {
        double[] slice = new double[values.GetLength(0)];

        for (int y = 0; y < slice.Length; y++)
        {
            slice[y] = values[y, columnIndex];
        }

        return slice;
    }

    public static double[] VerticalMean(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(Mean).ToArray();
    }

    public static double[] VerticalNanMean(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(NanMean).ToArray();
    }

    public static double[] VerticalStandardDeviation(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(StandardDeviation).ToArray();
    }

    public static double[] VerticalNanStandardDeviation(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(NanStandardDeviation).ToArray();
    }

    public static double[] VerticalStandardError(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(StandardError).ToArray();
    }

    public static double[] VerticalNanStandardError(double[,] values)
    {
        return Enumerable.Range(0, values.GetLength(1)).Select(x => VerticalSlice(values, x)).Select(NanStandardError).ToArray();
    }
}
