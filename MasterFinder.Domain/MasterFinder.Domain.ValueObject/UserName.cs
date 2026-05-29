using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class Username : ValueObject<string>
    {
        public Username(string name) : base(new UsernameValidator(), name) { }
    }
}