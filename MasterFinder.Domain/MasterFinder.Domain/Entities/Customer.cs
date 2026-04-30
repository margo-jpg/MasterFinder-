using MasterFinder.Domain.Base;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Entities
{
    public class Customer : Entity<Guid>
    {
        public Username Username { get; private set; }
        public PhoneNumber Phone { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<Order> _orders = [];
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        private Customer() { }

        public Customer(Username username, PhoneNumber phone) : base(Guid.NewGuid())
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            CreatedAt = DateTime.UtcNow;
        }

        public Order CreateOrder(OrderTitle title, OrderDescription? description = null)
        {
            var order = new Order(this, title, description, DateTime.UtcNow);
            _orders.Add(order);
            return order;
        }
    }
}

