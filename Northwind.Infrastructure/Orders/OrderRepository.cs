using Microsoft.EntityFrameworkCore;

using Northwind.Application.Data;
using Northwind.Domain.Orders;

namespace Northwind.Infrastructure.Orders
{
    public sealed class OrderRepository(
        IApplicationDbContext dbContext) : IOrderRepository
    {
        public async Task<string> GetOrderStatusAsync(
            int orderId,
            CancellationToken cancellationToken = default)
        {
            var order = await dbContext.Orders
                .AsNoTracking()
                .Where(x => x.Id == orderId)
                .Select(x => new
                {
                    x.OrderDate,
                    x.ShippedDate,
                    x.RequiredDate
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (order is null)
                return "Order Not Found";

            if (order.ShippedDate is null)
                return "Pending";

            if (order.ShippedDate.HasValue)
                return "Shipped";

            return "Processing";
        }

        public async Task<string> GetOrderDeliveryDateAsync(
            int orderId,
            CancellationToken cancellationToken = default)
        {
            var order = await dbContext.Orders
                .AsNoTracking()
                .Where(x => x.Id == orderId)
                .Select(x => new
                {
                    x.ShippedDate,
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (order is null)
                return "Order Not Found";

            if (order.ShippedDate is null)
                return "No delivery information is available.";

            if (order.ShippedDate < DateTime.UtcNow)
                return "The order has already been delivered on " + order.ShippedDate.ToString();

            return "Processing";
        }
    }
}
