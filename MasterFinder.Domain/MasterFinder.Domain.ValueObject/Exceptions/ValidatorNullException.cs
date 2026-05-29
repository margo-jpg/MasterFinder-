namespace MasterFinder.ValueObjects.Exceptions
{
    public class ValidatorNullException : ArgumentNullException
    {
        public ValidatorNullException(string paramName)
            : base(paramName, $"Валидатор \"{paramName}\" не может быть null") { }
    }
}