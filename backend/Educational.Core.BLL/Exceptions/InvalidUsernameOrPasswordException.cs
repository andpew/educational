﻿namespace Educational.Core.BLL.Exceptions;

public sealed class InvalidUsernameOrPasswordException : Exception
{
    public InvalidUsernameOrPasswordException() : base("Invalid username or password")
    {

    }
}
