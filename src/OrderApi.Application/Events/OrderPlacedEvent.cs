namespace OrderApi.Application.Events;

public record OrderPlacedEvent(Guid OrderId, int Version);