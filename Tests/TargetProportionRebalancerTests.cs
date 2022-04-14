using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Saturday.Finance;
using Saturday.Finance.Data;
using Saturday.Finance.Strategies;
using Tests.Mocks;

namespace Tests;

public class TargetProportionRebalancerTests
{
    [Test]
    public void TestRebalance()
    {
        IDataSource data = new MockDataSource();

        var inputPortfolio = Portfolio.Builder
            .With(new Stock("A", "X"), 15)
            .With(new Stock("B", "X"), 23)
            .With(new Stock("C", "X"), 40)
            .Build();

        var desiredValueProportions = new Dictionary<Stock, decimal>()
        {
            {new Stock("A", "X"), 0.70m},
            {new Stock("B", "X"), 0.20m},
            {new Stock("C", "X"), 0.10m}
        };
        
        var inputPortfolioValue = data.GetPrice(inputPortfolio);
        var balancer = new TargetProportionRebalancer(data, desiredValueProportions);
        var balancedPortfolio = balancer.Rebalance(inputPortfolio);
        
        
        // Assert that the output portfolio contains the same stocks as input.
        Assert.IsEmpty(inputPortfolio.Contents.Keys.Except(balancedPortfolio.Contents.Keys));
        Assert.IsEmpty(balancedPortfolio.Contents.Keys.Except(inputPortfolio.Contents.Keys));
        
        // Assert that the output portfolio has the same total value as the input.
        var balancedPortfolioValue = data.GetPrice(balancedPortfolio);
        var portfolioValueChange = Math.Abs(balancedPortfolioValue - inputPortfolioValue);
        Assert.IsTrue(portfolioValueChange < 0.01M);

        // Assert that the desired proportions have been achieved.
        foreach (var kvp in balancedPortfolio.Contents)
        {
            var expectedStockValue = balancedPortfolioValue * desiredValueProportions[kvp.Key];
            var actualStockValue = data.GetPrice(kvp.Key) * kvp.Value;
            Assert.IsTrue(Math.Abs(expectedStockValue - actualStockValue) < 0.01M);
        }
    }
}