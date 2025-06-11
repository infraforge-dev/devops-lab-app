namespace Core.ExceptionTypes
{
    public class ValidationException(string message) : Exception(message)
    { }
}
