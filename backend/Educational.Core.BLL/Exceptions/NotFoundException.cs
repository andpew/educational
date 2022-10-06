namespace Educational.Core.BLL.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName) : base($"Entity {entityName} Not Found")
    {

    }
}
