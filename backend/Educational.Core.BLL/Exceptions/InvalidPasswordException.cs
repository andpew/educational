namespace Educational.Core.BLL.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message) : base(message)
    {

    }
}
