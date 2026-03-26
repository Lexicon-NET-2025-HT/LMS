namespace Domain.Models.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message, string title = "Bad Request")
        : base(message, title, 403)
    {
    }
}
