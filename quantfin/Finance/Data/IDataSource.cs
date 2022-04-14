namespace Saturday.Finance.Data;

public interface IDataSource
{
    /// <summary>
    /// Gets the price of the given stock according to this data source.
    /// </summary>
    /// <param name="stock">The stock to query.</param>
    /// <returns>The price of the given stock.</returns>
    decimal GetPrice(Stock stock)
    {
        return GetPrice(stock, DateTimeOffset.Now);
    }

    /// <summary>
    /// Gets the value of the given portfolio according to this data source.
    /// </summary>
    /// <param name="portfolio">The portfolio to query.</param>
    /// <returns>The total value of all the contents in the given portfolio.</returns>
    decimal GetPrice(Portfolio portfolio)
    {
        return portfolio.Contents.Sum(kvp => GetPrice(kvp.Key, DateTimeOffset.Now) * kvp.Value);
    }

    /// <summary>
    /// Gets the price of the given stock at the given time according to this data source.
    /// </summary>
    /// <param name="stock">The stock to query.</param>
    /// <param name="time">The date for the price to be queried.</param>
    /// <returns>The price of the given stock on the given date.</returns>
    decimal GetPrice(Stock stock, DateTimeOffset time);

    /// <summary>
    /// Gets the value of the given portfolio at the given time according to this data source.
    /// </summary>
    /// <param name="portfolio">The portfolio to query.</param>
    /// <param name="time">The date for the price to be queried.</param>
    /// <returns>The total value of all the contents in the given portfolio.</returns>
    decimal GetPrice(Portfolio portfolio, DateTimeOffset time)
    {
        return portfolio.Contents.Sum(kvp => GetPrice(kvp.Key, time) * kvp.Value);
    }


    decimal GetMarketCapitalization(Stock stock, DateTimeOffset time);

    decimal GetMarketCapitalization(Stock stock)
    {
        return GetMarketCapitalization(stock, DateTimeOffset.Now);
    }
}