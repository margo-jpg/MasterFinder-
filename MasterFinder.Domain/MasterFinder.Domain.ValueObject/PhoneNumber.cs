using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class PhoneNumber : ValueObject<string>
    {
        public PhoneNumber(string phone) : base(new PhoneNumberValidator(), phone) { }
    }
}