namespace Saturday.Finance.Orders;

public class BuyOrder : IOrder
{
    /// <summary>
    /// The stock to buy.
    /// </summary>
    public Stock Stock { get; }

    /// <summary>
    /// The number of shares to buy.
    /// </summary>
    public decimal Amount { get; }

    public BuyOrder(Stock stock, decimal amount)
    {
        Stock = stock;
        Amount = amount;
        if (amount < 0)
            throw new ArgumentException("Cannot create a buy order for a negative quantity");
    }

    public override string ToString()
    {
        return $"Buy {Amount} shares of {Stock}";
    }

    public void Accept(IOrderVisitor visitor)
    {
        visitor.VisitBuyOrder(this);
    }
}