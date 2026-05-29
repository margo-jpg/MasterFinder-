using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.ValueObjects.Validators
{
    public class ResponseCommentValidator : IValidator<string?>
    {
        public const int MAX_LENGTH = 1000;

        public void Validate(string? value)
        {
            if (value != null && value.Length > MAX_LENGTH)
                throw new ArgumentLongValueException(nameof(value), value.Length, MAX_LENGTH);
        }
    }
}


