namespace Northwind.Domain.Orders
{
    public sealed class Order
    {
        public int Id { get; private set; }

        public int CustomerId { get; private set; }

        public int EmployeeId { get; private set; }

        public DateTime OrderDate { get; private set; }

        public DateTime RequiredDate { get; private set; }

        public DateTime? ShippedDate { get; private set; }
    }
}
