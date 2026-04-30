namespace MasterFinder.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string entityName, object id)
            : base($"{entityName} с ID {id} не найден") { }
    }
}