using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class Specialization : ValueObject<string>
    {
        public Specialization(string specialization) : base(new SpecializationValidator(), specialization) { }
    }
}