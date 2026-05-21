using MasterFinder.ValueObjects.Base;
using MasterFinder.ValueObjects.Exceptions;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.ValueObjects
{
    public class ResponseComment : ValueObject<string?>
    {
        private const int MaxLength = 1000;

        private ResponseComment(string? value) : base(new ResponseCommentValidator(), value)
        {
        }

        public static ResponseComment Create(string? value)
        {
            return new ResponseComment(value);
        }

        public static implicit operator string?(ResponseComment comment) => comment?.Value;
        public static implicit operator ResponseComment(string? value) => Create(value);
    }
}