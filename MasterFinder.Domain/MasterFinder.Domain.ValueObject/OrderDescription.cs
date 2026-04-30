using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class OrderDescription : ValueObject<string?>
    {
        public OrderDescription(string? description) : base(new OrderDescriptionValidator(), description) { }
    }
}