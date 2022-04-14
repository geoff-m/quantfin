namespace Saturday.Finance.Orders;

public interface IOrderVisitor
{
    void VisitBuyOrder(BuyOrder buyOrder);
    void VisitSellOrder(SellOrder sellOrder);
}