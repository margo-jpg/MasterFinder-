namespace MasterFinder.ValueObjects.Exceptions
{
    public class ArgumentShortValueException : FormatException
    {
        public ArgumentShortValueException(string paramName, int length, int minLength)
            : base($"Параметр \"{paramName}\" длиной {length} меньше минимума {minLength}") { }
    }
}