namespace Northwind.Domain.Orders
{
    public interface IOrderRepository
    {
        Task<string> GetOrderStatusAsync(
            int orderId,
            CancellationToken cancellationToken = default);

        Task<string> GetOrderDeliveryDateAsync(
            int orderId,
            CancellationToken cancellationToken = default);
    }
}
