using System;
using System.Collections.Generic;
using System.Linq;
using Saturday.Finance;
using Saturday.Finance.Orders;

namespace Tests.Mocks;

public class MockOrderExecutor
{
    public Portfolio ExecuteOrders(Portfolio input, IEnumerable<IOrder> orders)
    {
        var visitor = new OrderVisitor(input);
        return visitor.Go(orders);
    }

    private class OrderVisitor : IOrderVisitor
    {
        private Dictionary<Stock, decimal> port;

        public OrderVisitor(Portfolio input)
        {
            port = new Dictionary<Stock, decimal>(input.Contents);
        }

        public Portfolio Go(IEnumerable<IOrder> orders)
        {
            foreach (var order in orders)
                order.Accept(this);

            // Remove stocks with zero quantity.
            var toRemove = port.Where(kvp => kvp.Value == 0).ToList();
            foreach (var kvp in toRemove)
                port.Remove(kvp.Key);

            return new Portfolio(port);
        }

        public void VisitBuyOrder(BuyOrder buyOrder)
        {
            if (buyOrder.Amount < 0)
                throw new ArgumentException("Cannot execute buy order for negative amount!");
            port.TryGetValue(buyOrder.Stock, out var existing);

            port[buyOrder.Stock] = existing + buyOrder.Amount;
        }

        public void VisitSellOrder(SellOrder sellOrder)
        {
            if (sellOrder.Amount < 0)
                throw new ArgumentException("Cannot execute sell order for negative amount!");
            port.TryGetValue(sellOrder.Stock, out var existing);
            if (existing < sellOrder.Amount)
                throw new ArgumentException("Cannot sell more shares than are in the portfolio!");

            port[sellOrder.Stock] = existing - sellOrder.Amount;
        }
    }
}