using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Northwind.Domain.Orders;

namespace Northwind.Infrastructure.Orders
{
    public sealed class OrderConfigurations 
        : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("OrderID");

            builder.Property(x => x.CustomerId)
                .HasColumnName("CustomerID");

            builder.Property(x => x.EmployeeId)
                .HasColumnName("EmployeeID");

            builder.Property(x => x.OrderDate)
                .HasColumnName("OrderDate");

            builder.Property(x => x.RequiredDate)
                .HasColumnName("RequiredDate");

            builder.Property(x => x.ShippedDate)
                .HasColumnName("ShippedDate");
        }
    }
}
