namespace Radancy.Api.Models;

public class ValidationFailed
{
    public string PropertyName { get; }
    public string Message { get; }

    public ValidationFailed(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }
}