using MasterFinder.Domain.Base;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Entities
{
    public class Execution : Entity<Guid>
    {
        public Order Order { get; private set; }
        public Executor Executor { get; private set; }
        public DateTime StartedAt { get; private set; }    //не nullable
        public DateTime? CompletedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public CancelReason? CancelReason { get; private set; }

        protected Execution() { }   //исправила

        // Защищённый конструктор (для EF)
        protected Execution(Guid id, Order order, Executor executor, DateTime startedAt,
            DateTime? completedAt, DateTime? cancelledAt, string? cancelReason)
            : base(id)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            StartedAt = startedAt;
            CompletedAt = completedAt;
            CancelledAt = cancelledAt;
            CancelReason = cancelReason;
        }

        // Публичный конструктор
        public Execution(Order order, Executor executor)
            : this(Guid.NewGuid(), order, executor, DateTime.UtcNow, null, null, null)
        {
        }

        // принимает Executor и возвращает bool
        public bool Complete(Executor executor)
        {
            if (executor != Executor)
                throw new BusinessRuleViolationException("Только назначенный исполнитель может завершить заказ");

            if (CancelledAt != null)
                throw new BusinessRuleViolationException("Отмененное выполнение нельзя завершить");

            if (CompletedAt != null)
                return false; // уже завершено

            CompletedAt = DateTime.UtcNow;
            return true;
        }

        // принимает причину и исполнителя, возвращает bool
        public bool Cancel(Executor executor, CancelReason? reason = null)
        {
            if (executor != Executor)
                throw new BusinessRuleViolationException("Только назначенный исполнитель может отменить заказ");

            if (CompletedAt != null)
                throw new BusinessRuleViolationException("Завершенное выполнение нельзя отменить");

            if (CancelledAt != null)
                return false; // уже отменено

            CancelledAt = DateTime.UtcNow;
            CancelReason = reason;
            return true;
        }
    }
}
    