using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class OrderTitle : ValueObject<string>
    {
        public OrderTitle(string value) : base(new OrderTitleValidator(), value) { }

        public static OrderTitle Create(string value)
        {
            return new OrderTitle(value);
        }

        public static implicit operator string?(OrderTitle title) => title?.Value;
        public static implicit operator OrderTitle(string? value) => new OrderTitle(value);
    }
}