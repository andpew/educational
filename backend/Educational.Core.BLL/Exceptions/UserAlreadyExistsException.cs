namespace Educational.Core.BLL.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base("User with given email or username already exists.")
    {

    }
}
