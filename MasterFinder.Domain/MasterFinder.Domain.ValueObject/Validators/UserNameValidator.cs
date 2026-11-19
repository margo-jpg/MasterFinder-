using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class UsernameValidator : IValidator<string>
    {
        public const int MIN_LENGTH = 2;
        public const int MAX_LENGTH = 100;

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