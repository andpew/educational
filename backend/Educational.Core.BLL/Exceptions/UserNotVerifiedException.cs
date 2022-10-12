namespace Educational.Core.BLL.Exceptions;

public sealed class UserNotVerifiedException : Exception
{
    public UserNotVerifiedException() : base("User is not verified")
    {

    }
}
