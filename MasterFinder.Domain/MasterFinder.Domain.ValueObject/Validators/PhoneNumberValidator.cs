using System.Text.RegularExpressions;
using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class PhoneNumberValidator : IValidator<string>
    {
        private const int MinLength = 10;
        private const int MaxLength = 15;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value));

            var regex = new Regex(@"^\+?[0-9]{10,15}$");
            if (!regex.IsMatch(value))
                throw new FormatException($"Номер телефона \"{value}\" имеет неверный формат");

            if (value.Length > MaxLength)
                throw new ArgumentLongValueException(nameof(value), value.Length, MaxLength);

            if (value.Length < MinLength)
                throw new ArgumentShortValueException(nameof(value), value.Length, MinLength);
        }
    }
}
