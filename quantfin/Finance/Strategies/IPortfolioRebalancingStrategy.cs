namespace Saturday.Finance.Strategies;

public interface IPortfolioRebalancingStrategy
{
    /// <summary>
    /// Creates a new portfolio that represents a rebalancing of the input one.
    /// </summary>
    /// <param name="existing">The portfolio to rebalance.</param>
    /// <returns>A rebalanced version of the input portfolio.</returns>
    Portfolio Rebalance(Portfolio existing);
}