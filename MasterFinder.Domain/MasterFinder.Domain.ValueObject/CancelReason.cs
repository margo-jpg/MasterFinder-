using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class CancelReason : ValueObject<string?>
    {
        private const int MaxLength = 500;

        private CancelReason(string? value) : base(new CancelReasonValidator(), value)
        {
        }

        public static CancelReason Create(string? value)
        {
            return new CancelReason(value);
        }

        public static implicit operator string?(CancelReason reason) => reason?.Value;
        public static implicit operator CancelReason(string? value) => Create(value);
    }
}


