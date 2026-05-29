namespace MasterFinder.ValueObjects.Exceptions
{
    public class ArgumentLongValueException : FormatException
    {
        public ArgumentLongValueException(string paramName, int length, int maxLength)
            : base($"Параметр \"{paramName}\" длиной {length} превышает максимум {maxLength}") { }
    }
}