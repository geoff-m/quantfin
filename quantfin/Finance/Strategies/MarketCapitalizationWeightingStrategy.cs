using Saturday.Finance.Data;

namespace Saturday.Finance.Strategies;

public class MarketCapitalizationWeightingStrategy
{
    public static Portfolio MakePortfolio(IDataSource data, IList<Stock> stocks,
        decimal totalPortfolioValue)
    {
        var marketCaps = stocks.ToDictionary(
            stock => stock,
            stock => data.GetMarketCapitalization(stock));

        var totalMarketCap = marketCaps.Values.Sum();
        foreach (var stock in marketCaps.Keys)
            marketCaps[stock] /= totalMarketCap;
        
        return ValueProportionWeightingStrategy.MakePortfolio(data, marketCaps, totalPortfolioValue);
    }
}