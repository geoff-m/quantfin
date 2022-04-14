using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Saturday.Finance;
using Saturday.Finance.Data;
using Saturday.Finance.Strategies;
using Tests.Mocks;

namespace Tests;

public class MarketCapitalizationWeightingStrategyTests
{
    [Test]
    public void Test()
    {
        IDataSource data = new MockDataSource();

        var stocks = new List<Stock>()
        {
            new ("A", "X"),
            new ("B", "X"),
            new ("C", "X")
        };

        // Create a portfolio of A, B, and C where each is weighted by its market capitalization.
        const decimal PORTFOLIO_VALUE = 1000M;
        var portfolio = MarketCapitalizationWeightingStrategy.MakePortfolio(data, stocks, PORTFOLIO_VALUE);
        
        
        // Assert that the output portfolio contains the same stocks as input.
        Assert.IsEmpty(portfolio.Contents.Keys.Except(stocks));
        Assert.IsEmpty(stocks.Except(portfolio.Contents.Keys));
        
        // Assert that the output portfolio has the desired total value.
        var actualPortfolioValue = data.GetPrice(portfolio);
        var portfolioValueChange = Math.Abs(actualPortfolioValue - PORTFOLIO_VALUE);
        Assert.IsTrue(portfolioValueChange < 0.01M);

        // Assert that the desired proportions have been achieved.
        var marketCaps = stocks.ToDictionary(
            stock => stock,
            stock => data.GetMarketCapitalization(stock));
        var totalMarketCap = marketCaps.Values.Sum();
        foreach (var stock in stocks)
            marketCaps[stock] /= totalMarketCap;
        foreach (var kvp in portfolio.Contents)
        {
            var expectedStockValue = marketCaps[kvp.Key] * PORTFOLIO_VALUE;
            var actualStockValue = data.GetPrice(kvp.Key) * kvp.Value;
            Assert.IsTrue(Math.Abs(expectedStockValue - actualStockValue) < 0.01M);
        }
    }
}