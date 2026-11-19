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

        private readonly ICollection<Order> _orders = new List<Order>();    //исправила
        public IReadOnlyCollection<Order> Orders => _orders.ToList().AsReadOnly();

        protected Customer() { }

        // Конструктор с полным количеством параметров (для EF)
        protected Customer(Guid id, Username username, PhoneNumber phone, DateTime createdAt)
            : base(id)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            CreatedAt = createdAt;
        }

        // Публичный конструктор (для создания через код)
        public Customer(Username username, PhoneNumber phone)
            : this(Guid.NewGuid(), username, phone, DateTime.UtcNow)
        {
        }
        public Order CreateOrder(OrderTitle title, OrderDescription? description = null)
        {
            var order = new Order(title, description, DateTime.UtcNow,this);
            _orders.Add(order);
            return order;
        }
    }
}

