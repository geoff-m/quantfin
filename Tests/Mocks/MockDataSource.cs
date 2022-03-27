using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Interpolation;
using NUnit.Framework;
using Saturday.Finance;
using Saturday.Finance.Data;

namespace Tests.Mockups;

/// <summary>
/// A mock data source that provides random data.
/// Data is constant for each (stock, time) tuple.
/// </summary>
public class MockDataSource : IDataSource
{
    // Year x (stock x (month x price))
    private Dictionary<int, Dictionary<Stock, IInterpolation>> data = new();

    private const double MILLISECONDS_PER_MONTH = 2629800000.0;

    public decimal GetPrice(Stock stock, DateTimeOffset time)
    {
        var year = time.Year;
        var month = time.Month;
        // Look up stored data, generating it if necessary.
        if (!data.TryGetValue(year, out var monthlyDataByStock))
        {
            monthlyDataByStock = data[year] = new Dictionary<Stock, IInterpolation>();
        }

        if (!monthlyDataByStock.TryGetValue(stock, out var monthlyData))
        {
            var random = new Random(stock.GetHashCode());
            var yearMean = random.NextDouble() * 500 + 1;
            var monthSamples = Normal.Samples(random, yearMean, 250)
                .Take(12)
                .Select(p => Math.Abs(p));
            monthlyData = monthlyDataByStock[stock] = Interpolate.CubicSplineRobust(
                Enumerable.Range(0, 12).Select(m => (double) m),
                monthSamples);
        }

        // Interpolate between the months.
        var monthStart = new DateTimeOffset(year, month + 1, 1, 0, 0, 0, time.Offset);
        var timeSinceMonthStart = time - monthStart;
        return (decimal) monthlyData.Interpolate(timeSinceMonthStart.TotalMilliseconds / MILLISECONDS_PER_MONTH);

    }
}