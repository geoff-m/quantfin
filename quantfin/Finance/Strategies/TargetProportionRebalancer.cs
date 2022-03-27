using Saturday.Finance.Data;

namespace Saturday.Finance.Strategies;

public class TargetProportionRebalancer : IPortfolioRebalancingStrategy
{
    private IDataSource data { get; }
    private IDictionary<Stock, decimal> desiredProportions;

    /// <summary>
    /// Creates a new <see cref="TargetProportionRebalancer"/> with the given proportions.
    /// </summary>
    /// <param name="data">The data source to use for rebalancing.</param>
    /// <param name="desiredProportions">
    /// A dictionary mapping each portfolio stock to its desired proportion of the portfolio's value.
    /// The values in this dictionary must sum to 1.
    /// </param>
    public TargetProportionRebalancer(IDataSource data, IDictionary<Stock, decimal> desiredProportions)
    {
        this.data = data;
        this.desiredProportions = desiredProportions;
        var unity = desiredProportions.Values.Sum();
        if (unity != 1)
            throw new ArgumentException("The given proportions must sum to unity.");
    }
    
    public Portfolio Rebalance(Portfolio existing)
    {
        // First, a sanity check.
        // Check that the set of stocks is the one we were expecting.
        if (!existing.Contents.Keys.OrderBy(s => s)
                .SequenceEqual(desiredProportions.Keys.OrderBy(s => s)))
            throw new ArgumentException(
                "The given portfolio doesn't contain the same set of stocks as our desired proportions.");
        
        // Get the value of the portfolio overall.
        var portfolioValue = data.GetPrice(existing);
        
        // Set each stock to its desired value.
        var output = new Dictionary<Stock, decimal>();
        foreach (var stock in existing.Contents.Keys)
        {
            var desiredStockValue = desiredProportions[stock] * portfolioValue;
            var neededShareAmount = desiredStockValue / data.GetPrice(stock);
            output.Add(stock, neededShareAmount);
        }

        return new Portfolio(output);
    }
}