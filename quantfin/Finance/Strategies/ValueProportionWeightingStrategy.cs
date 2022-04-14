using Saturday.Finance.Data;

namespace Saturday.Finance.Strategies;

public class ValueProportionWeightingStrategy
{
    public static Portfolio MakePortfolio(IDataSource data, IDictionary<Stock, decimal> stockProportions,
        decimal totalPortfolioValue)
    {
        var port = new Dictionary<Stock, decimal>();
        var prices = stockProportions.Keys.ToDictionary(
            stock => stock,
            stock => data.GetPrice(stock));

        foreach (var kvp in stockProportions)
        {
            var stockValue = kvp.Value * totalPortfolioValue;
            var stockPrice = prices[kvp.Key];
            var stockQuantity = stockValue / stockPrice;
            port.Add(kvp.Key, stockQuantity);
        }
        return new Portfolio(port);
    }
}