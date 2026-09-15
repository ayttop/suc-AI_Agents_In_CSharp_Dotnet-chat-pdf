using Microsoft.EntityFrameworkCore;

using Northwind.Domain.Orders;

namespace Northwind.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
