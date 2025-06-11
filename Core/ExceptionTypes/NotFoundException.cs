namespace Core.ExceptionTypes
{
    public class NotFoundException(string message) : Exception(message)
    { }
}
