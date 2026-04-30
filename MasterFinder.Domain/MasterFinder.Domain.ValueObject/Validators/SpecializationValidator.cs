using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class SpecializationValidator : IValidator<string>
    {
        private const int MaxLength = 200;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentLongValueException(nameof(value), value.Length, MaxLength);
        }
    }
}
