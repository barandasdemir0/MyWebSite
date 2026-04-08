namespace SharedKernel.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(string message) : base(message)
    {
    }
}
