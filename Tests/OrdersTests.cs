using NUnit.Framework;
using Saturday.Finance;
using Tests.Mocks;

namespace Tests;

public class OrdersTests
{
    [Test]
    public void Test()
    {
        var beforePortfolio = Portfolio.Builder
            .With(new Stock("A", "X"), 15)
            .With(new Stock("B", "X"), 23)
            .With(new Stock("C", "X"), 40)
            .Build();

        
        var afterPortfolio = Portfolio.Builder
            .With(new Stock("A", "X"), 20)
            .With(new Stock("B", "X"), 23)
            .With(new Stock("D", "X"), 5)
            .Build();

        var orders = Portfolio.MakeChangeOrders(beforePortfolio, afterPortfolio);
        
        var paper = new MockOrderExecutor();
        var changedPortfolio = paper.ExecuteOrders(beforePortfolio, orders);

        Assert.AreEqual(afterPortfolio, changedPortfolio);
    }
}