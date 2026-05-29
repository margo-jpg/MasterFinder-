using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class OrderDescription : ValueObject<string?>
    {
        public OrderDescription(string? value) : base(new OrderDescriptionValidator(), value) { }

        public static OrderDescription Create(string? value)
        {
            return new OrderDescription(value);
        }

        public static implicit operator string?(OrderDescription description) => description?.Value;
        public static implicit operator OrderDescription(string? value) => new OrderDescription(value);
    }
}