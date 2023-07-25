namespace Domain.Configurations;

public class EventBusSettings
{
    public const string EventBus = "EventBus";
    public string? HostName { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? EventQueue { get; set; }
}
