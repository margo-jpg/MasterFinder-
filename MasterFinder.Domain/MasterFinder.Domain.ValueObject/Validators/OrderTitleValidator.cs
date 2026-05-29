using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class OrderTitleValidator : IValidator<string>
    {
        public const int MIN_LENGTH = 3;
        public const int MAX_LENGTH = 200;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value));

            if (value.Length > MAX_LENGTH)
                throw new ArgumentLongValueException(nameof(value), value.Length, MAX_LENGTH);

            if (value.Length < MIN_LENGTH)
                throw new ArgumentShortValueException(nameof(value), value.Length, MIN_LENGTH);
        }
    }
}


