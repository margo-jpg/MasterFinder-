using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class OrderDescriptionValidator : IValidator<string?>
    {
        public const int MAX_LENGTH = 5000;

        public void Validate(string? value)
        {
            if (value != null && value.Length > MAX_LENGTH)
                throw new ArgumentLongValueException(nameof(value), value.Length, MAX_LENGTH);
        }
    }
}