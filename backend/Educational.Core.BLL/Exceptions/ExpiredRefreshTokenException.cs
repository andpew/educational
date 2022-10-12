namespace Educational.Core.BLL.Exceptions;

public sealed class ExpiredRefreshTokenException : Exception
{
    public ExpiredRefreshTokenException() : base("Refresh token has expired.")
    {

    }
}
