using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class ResponseCommentValidator : IValidator<string?>
    {
        private const int MaxLength = 1000;

        public void Validate(string? value)
        {
            if (value != null && value.Length > MaxLength)
                throw new ArgumentLongValueException(nameof(value), value.Length, MaxLength);
        }
    }
}
