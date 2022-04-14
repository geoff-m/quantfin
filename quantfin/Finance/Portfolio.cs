using Saturday.Finance.Orders;

namespace Saturday.Finance;

public class Portfolio
{
    /// <summary>
    /// Each key represents a stock.
    /// Each value represents the number of shares this portfolio holds of that stock.
    /// </summary>
    public IReadOnlyDictionary<Stock, decimal> Contents { get; }

    public Portfolio(IReadOnlyDictionary<Stock, decimal> contents)
    {
        Contents = contents;
    }

    /// <summary>
    /// Creates a list of orders that would change <paramref name="actualPortfolio"/> into <paramref name="newPortfolio"/>.
    /// </summary>
    /// <param name="actualPortfolio"></param>
    /// <param name="newPortfolio"></param>
    /// <returns></returns>
    public static IList<IOrder> MakeChangeOrders(Portfolio actualPortfolio, Portfolio newPortfolio)
    {
        var orders = new List<IOrder>();
        foreach (var stock in actualPortfolio.Contents.Keys.Union(newPortfolio.Contents.Keys))
        {
            actualPortfolio.Contents.TryGetValue(stock, out var actualAmount);
            newPortfolio.Contents.TryGetValue(stock, out var newAmount);
            if (newAmount < actualAmount)
            {
                orders.Add(new SellOrder(stock, actualAmount - newAmount));
            }
            else if (newAmount > actualAmount)
            {
                orders.Add(new BuyOrder(stock, newAmount - actualAmount));
            }
        }

        return orders;
    }

    public static PortfolioBuilder Builder => new PortfolioBuilder();

    public class PortfolioBuilder
    {
        private Dictionary<Stock, decimal> data = new();

        public PortfolioBuilder With(Stock stock, decimal shares)
        {
            data.Add(stock, shares);
            return this;
        }

        public Portfolio Build() => new Portfolio(data);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Portfolio other)
            return false;
        return Contents.Count == other.Contents.Count
               && !Contents.Except(other.Contents).Any();
    }

    public override int GetHashCode()
    {
        return Contents.GetHashCode();
    }
}