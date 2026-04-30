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

        private readonly List<Response> _responses = [];
        public IReadOnlyCollection<Response> Responses => _responses.AsReadOnly();

        public Execution? Execution { get; private set; }

        private Order() { }

        public Order(Customer customer, OrderTitle title, OrderDescription? description, DateTime createdAt) : base(Guid.NewGuid())
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? new OrderDescription(null);
            Status = OrderStatus.Open;
            CreatedAt = createdAt;
        }

        public void AddResponse(Response response)
        {
            if (!_responses.Contains(response))
                _responses.Add(response);
        }

        public void AcceptResponse(Guid responseId)
        {
            if (Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Заказ уже не в статусе Open");

            var response = _responses.FirstOrDefault(r => r.Id == responseId);
            if (response == null)
                throw new NotFoundException("Отклик", responseId);

            response.Accept();
        }

        public void StartExecution(Executor executor)
        {
            if (Status != OrderStatus.Open)
                throw new BusinessRuleViolationException("Заказ должен быть открыт");

            var acceptedResponse = _responses.FirstOrDefault(r =>
                r.Executor.Id == executor.Id && r.Status == ResponseStatus.Accepted);

            if (acceptedResponse == null)
                throw new BusinessRuleViolationException("Этот исполнитель не был принят на заказ");

            Status = OrderStatus.InProgress;
            Execution = new Execution(this, executor);
        }

        public void Complete()
        {
            if (Status != OrderStatus.InProgress)
                throw new BusinessRuleViolationException("Заказ не в работе");

            Status = OrderStatus.Completed;
            Execution?.Complete();
        }

        public void Cancel(string? reason = null)
        {
            if (Status == OrderStatus.Completed)
                throw new BusinessRuleViolationException("Завершенный заказ нельзя отменить");

            Status = OrderStatus.Cancelled;
            Execution?.Cancel(reason);
        }
    }
}