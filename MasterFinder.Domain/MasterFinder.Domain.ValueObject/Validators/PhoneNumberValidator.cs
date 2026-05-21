using System.Text.RegularExpressions;
using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class PhoneNumberValidator : IValidator<string>
    {
        public const int MIN_LENGTH = 10;
        public const int MAX_LENGTH = 15;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value));

            var regex = new Regex(@"^\+?[0-9]{10,15}$");
            if (!regex.IsMatch(value))
                throw new FormatException($"Номер телефона \"{value}\" имеет неверный формат");

            if (value.Length > MAX_LENGTH)
                throw new ArgumentLongValueException(nameof(value), value.Length, MAX_LENGTH);

            if (value.Length < MIN_LENGTH)
                throw new ArgumentShortValueException(nameof(value), value.Length, MIN_LENGTH);
        }
    }
}




