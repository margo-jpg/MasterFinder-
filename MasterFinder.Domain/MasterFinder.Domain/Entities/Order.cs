using MasterFinder.Domain.Base;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;            

namespace MasterFinder.Domain.Entities
{
    public class Order : Entity<Guid>
    {
        public Customer Customer { get; private set; }
        public OrderTitle Title { get; private set; }
        public OrderDescription Description { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Execution? Execution { get; private set; }

        private readonly ICollection<Response> _responses = new List<Response>(); //исправила
        public IReadOnlyCollection<Response> Responses => _responses.ToList().AsReadOnly();

        protected Order() { }

        // Защищённый конструктор (для EF)
        protected Order(Guid id, Customer customer, OrderTitle title, OrderDescription description,
            OrderStatus status, DateTime createdAt, Execution? execution = null)
            : base(id)
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? new OrderDescription(null);
            Status = status;
            CreatedAt = createdAt;
            Execution = execution;
        }

        // Публичный конструктор
        public Order(OrderTitle title, OrderDescription? description, DateTime createdAt, Customer customer)
            : this(Guid.NewGuid(), customer, title, description ?? new OrderDescription(null),
                  OrderStatus.Open, createdAt, null)
        {
        }

        public Response AddResponse(Executor executor, ResponseComment? comment = null)
        {
            if (_responses.Any(r => r.Executor == executor))
                throw new BusinessRuleViolationException("Этот исполнитель уже откликался");

            var response = new Response(this, executor, comment);
            _responses.Add(response);
            return response;
        }

        public void AcceptResponse(Response response, Customer customer)
        {
            if (Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Заказ уже не в статусе Open");

            if (!_responses.Contains(response))
                throw new NotFoundException("Отклик");

            response.Accept(customer);
        }

        public void StartExecution(Executor executor)
        {
            if (Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Заказ должен быть открыт");

            var acceptedResponse = _responses.FirstOrDefault(r =>
                r.Executor == executor && r.Status == ResponseStatus.Accepted);

            if (acceptedResponse == null)
                throw new BusinessRuleViolationException("Этот исполнитель не был принят на заказ");

            Status = OrderStatus.InProgress;
            Execution = new Execution(this, executor);
        }

        public void Complete(Executor executor)
        {
            if (Status != OrderStatus.InProgress)
                throw new BusinessRuleViolationException("Заказ не в работе");

            if (Execution == null)
                throw new BusinessRuleViolationException("Нет информации о выполнении");

            var completed = Execution.Complete(executor);
            if (completed)
                Status = OrderStatus.Completed;
        }

        public void Cancel(Executor executor, CancelReason? reason = null)
        {
            if (Status == OrderStatus.Completed)
                throw new BusinessRuleViolationException("Завершенный заказ нельзя отменить");

            if (Execution != null && Execution.StartedAt != null && Execution.CompletedAt == null)
            {
                Execution.Cancel(executor, reason);
            }

            Status = OrderStatus.Cancelled;
        }
    }
}