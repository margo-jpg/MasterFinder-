namespace MasterFinder.ValueObjects.Exceptions
{
    public class ArgumentNullOrWhiteSpaceException : ArgumentNullException
    {
        public ArgumentNullOrWhiteSpaceException(string paramName)
            : base(paramName, $"Параметр \"{paramName}\" не может быть пустым") { }
    }
}