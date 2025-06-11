namespace Core.ExceptionTypes
{
    public class OperationFailedException(string message) : Exception(message)
    { }
}
