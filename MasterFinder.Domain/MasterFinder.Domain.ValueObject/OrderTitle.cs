using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class OrderTitle : ValueObject<string>
    {
        public OrderTitle(string title) : base(new OrderTitleValidator(), title) { }
    }
}