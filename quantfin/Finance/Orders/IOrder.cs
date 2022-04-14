namespace Saturday.Finance.Orders;

public interface IOrder
{
    void Accept(IOrderVisitor visitor);
}