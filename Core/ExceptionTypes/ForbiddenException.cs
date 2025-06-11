namespace Core.ExceptionTypes
{
    public class ForbiddenException(string message) : Exception(message)
    { }
}
