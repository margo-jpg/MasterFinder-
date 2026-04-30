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

        private readonly List<Response> _responses = [];
        public IReadOnlyCollection<Response> Responses => _responses.AsReadOnly();

        private Executor() { }

        public Executor(Username username, PhoneNumber phone, Specialization specialization) : base(Guid.NewGuid())
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Specialization = specialization ?? throw new ArgumentNullException(nameof(specialization));
            CreatedAt = DateTime.UtcNow;
        }

        public Response RespondToOrder(Order order, string? comment = null)
        {
            if (order.Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Нельзя откликнуться на закрытый заказ");

            if (_responses.Any(r => r.OrderId == order.Id))
                throw new BusinessRuleViolationException("Вы уже откликались на этот заказ");

            var response = new Response(order, this, comment);
            _responses.Add(response);
            order.AddResponse(response);
            return response;
        }
    }
}