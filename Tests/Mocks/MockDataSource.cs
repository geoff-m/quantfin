using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Interpolation;
using Saturday.Finance;
using Saturday.Finance.Data;

namespace Tests.Mocks;

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
        var monthlyData = EnsureData(stock, year);
        
        // Interpolate between the months.
        var monthStart = new DateTimeOffset(year, month + 1, 1, 0, 0, 0, time.Offset);
        var timeSinceMonthStart = time - monthStart;
        return (decimal) monthlyData.Interpolate(timeSinceMonthStart.TotalMilliseconds / MILLISECONDS_PER_MONTH);

    }

    private IInterpolation EnsureData(Stock stock, int year)
    {
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

        return monthlyData;
    }

    public decimal GetMarketCapitalization(Stock stock, DateTimeOffset time)
    {
        var interp = EnsureData(stock, time.Year);
        return (decimal)(interp.Interpolate(0) * 100000);
    }
}