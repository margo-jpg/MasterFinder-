using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class CancelReasonValidator : IValidator<string?>
    {
        private const int MaxLength = 500;

        public void Validate(string? value)
        {
            if (value != null && value.Length > MaxLength)
                throw new ArgumentLongValueException(nameof(value), value.Length, MaxLength);
        }
    }
}