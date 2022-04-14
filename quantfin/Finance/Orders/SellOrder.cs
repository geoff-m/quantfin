namespace Saturday.Finance.Orders;

public class SellOrder : IOrder
{
    /// <summary>
    /// The stock to sell.
    /// </summary>
    public Stock Stock { get; }

    /// <summary>
    /// The number of shares to sell.
    /// </summary>
    public decimal Amount { get; }

    public SellOrder(Stock stock, decimal amount)
    {
        Stock = stock;
        Amount = amount;
        if (amount < 0)
            throw new ArgumentException("Cannot create a sell order for a negative quantity");
    }
    
    public void Accept(IOrderVisitor visitor)
    {
        visitor.VisitSellOrder(this);
    }

    public override string ToString()
    {
        return $"Sell {Amount} shares of {Stock}";
    }
}