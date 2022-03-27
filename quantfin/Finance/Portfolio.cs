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
}