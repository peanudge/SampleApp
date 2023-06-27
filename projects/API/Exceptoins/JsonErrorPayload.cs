namespace API.Exceptions;

public class JsonErrorPayload
{
    public int EventId { get; set; }
    public object? DetailedMessage { get; set; }
}
