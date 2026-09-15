using Northwind.Domain.Orders;

using System.ComponentModel;

namespace Northwind.Infrastructure.AI.Tools
{
    public sealed class OrderTools (
        IOrderRepository orderRepository)
    {
        [Description("Gets the current status of an order.")]
        public async Task<string> GetOrderStatusAsync(
            [Description("The order id.")] int orderId,
            CancellationToken cancellationToken)
        {
            return await orderRepository.GetOrderStatusAsync(
                orderId,
                cancellationToken);
        }

        [Description("Gets the estimated delivery date for an order.")]
        public async Task<string> GetEstimatedDeliveryDateAsync(
            [Description("The order id.")] int orderId,
            CancellationToken cancellationToken)
        {
            return await orderRepository.GetOrderDeliveryDateAsync(
                orderId,
                cancellationToken);
        }
    }
}
