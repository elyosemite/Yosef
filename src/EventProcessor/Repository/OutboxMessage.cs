namespace EventProcessor.Repository;

public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; }
    public string Type { get; init; } = default!;
    public string Payload { get; init; } = default!;
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}