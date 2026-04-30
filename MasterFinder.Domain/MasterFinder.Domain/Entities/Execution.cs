using MasterFinder.Domain.Base;
using MasterFinder.Domain.Exceptions;

namespace MasterFinder.Domain.Entities
{
    public class Execution : Entity<Guid>
    {
        public Order Order { get; private set; }
        public Executor Executor { get; private set; }
        public Guid OrderId => Order.Id;
        public Guid ExecutorId => Executor.Id;
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public string? CancelReason { get; private set; }

        private Execution() { }

        public Execution(Order order, Executor executor) : base(Guid.NewGuid())
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            StartedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (CancelledAt != null)
                throw new BusinessRuleViolationException("Отмененное выполнение нельзя завершить");

            CompletedAt = DateTime.UtcNow;
        }

        public void Cancel(string? reason = null)
        {
            if (CompletedAt != null)
                throw new BusinessRuleViolationException("Завершенное выполнение нельзя отменить");

            CancelledAt = DateTime.UtcNow;
            CancelReason = reason;
        }
    }
}