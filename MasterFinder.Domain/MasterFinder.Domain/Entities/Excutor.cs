using MasterFinder.Domain.Base;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Entities
{
    public class Executor : Entity<Guid>
    {
        public Username Username { get; private set; }
        public PhoneNumber Phone { get; private set; }
        public Specialization Specialization { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly ICollection<Response> _responses = new List<Response>();       //исправила
        public IReadOnlyCollection<Response> Responses => _responses.ToList().AsReadOnly();

        private readonly ICollection<Execution> _executions = new List<Execution>();    //наверно надо
        public IReadOnlyCollection<Execution> Executions => _executions.ToList().AsReadOnly();
        protected Executor() { }

        // Защищённый конструктор (для EF)
        protected Executor(Guid id, Username username, PhoneNumber phone,
            Specialization specialization, DateTime createdAt)
            : base(id)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Specialization = specialization ?? throw new ArgumentNullException(nameof(specialization));
            CreatedAt = createdAt;
        }

        // Публичный конструктор
        public Executor(Username username, PhoneNumber phone, Specialization specialization)
            : this(Guid.NewGuid(), username, phone, specialization, DateTime.UtcNow)
        {
        }

        public Response RespondToOrder(Order order, ResponseComment? comment = null)
        {
            if (order.Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Нельзя откликнуться на закрытый заказ");

            if (_responses.Any(r => r.Order == order))
                throw new BusinessRuleViolationException("Вы уже откликались на этот заказ");

            var response = order.AddResponse(this, comment);
            _responses.Add(response);
            
            return response;
        }
    }
}