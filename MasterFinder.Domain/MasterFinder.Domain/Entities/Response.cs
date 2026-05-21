using MasterFinder.Domain.Base;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;
using System;

namespace MasterFinder.Domain.Entities
{
    public class Response : Entity<Guid>
    {
        public Order Order { get; private set; }
        public Executor Executor { get; private set; }
        public ResponseComment? Comment { get; private set; }
        public ResponseStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Response() { }    //исправила

        // Защищённый конструктор (для EF)
        protected Response(Guid id, Order order, Executor executor, string? comment,
            ResponseStatus status, DateTime createdAt)
            : base(id)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            Comment = comment;
            Status = status;
            CreatedAt = createdAt;
        }

        // Публичный конструктор
        public Response(Order order, Executor executor, string? comment = null)
            : this(Guid.NewGuid(), order, executor, comment, ResponseStatus.Pending, DateTime.UtcNow)
        {
        }

        // отозвать может только исполнитель, который создал отклик
        public void Withdraw(Executor executor)
        {
            if (executor != Executor)
                throw new BusinessRuleViolationException("Только автор отклика может его отозвать");

            if (Status != ResponseStatus.Pending)
                throw new BusinessRuleViolationException("Можно отозвать только ожидающий отклик");

            Status = ResponseStatus.Withdrawn;
        }

        // принять может только заказчик, владелец заказа
        public void Accept(Customer customer)
        {
            if (customer != Order.Customer)
                throw new BusinessRuleViolationException("Только заказчик может принять отклик");

            if (Status != ResponseStatus.Pending)
                throw new BusinessRuleViolationException("Можно принять только ожидающий отклик");

            Status = ResponseStatus.Accepted;
        }

        // отклонить может только заказчик, владелец заказа
        public void Reject(Customer customer)
        {
            if (customer != Order.Customer)
                throw new BusinessRuleViolationException("Только заказчик может отклонить отклик");

            if (Status != ResponseStatus.Pending)
                throw new BusinessRuleViolationException("Можно отклонить только ожидающий отклик");

            Status = ResponseStatus.Rejected;
        }
    }
}