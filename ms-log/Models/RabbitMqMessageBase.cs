﻿namespace ms_log.Models;

public record RabbitMqMessageBase<T>
{
    public required string ApplicationName { get; set; }
    
    public required string RoutingKey { get; set; }
    
    public T? Payload { get; set; }

    public required DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
