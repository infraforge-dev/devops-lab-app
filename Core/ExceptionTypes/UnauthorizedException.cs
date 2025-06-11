namespace Core.ExceptionTypes
{
    public class UnauthorizedException(string message) : Exception(message)
    { }
}
