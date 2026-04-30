using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class OrderTitleValidator : IValidator<string>
    {
        private const int MinLength = 3;
        private const int MaxLength = 200;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentLongValueException(nameof(value), value.Length, MaxLength);

            if (value.Length < MinLength)
                throw new ArgumentShortValueException(nameof(value), value.Length, MinLength);
        }
    }
}
