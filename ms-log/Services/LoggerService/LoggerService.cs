
using ms_log.Data;
using ms_log.Models;
using ms_recip.Ms_recip.Models;
using System.Text.Json;

namespace ms_log.Services.LoggerService;

public class LoggerService(DatabaseContext _databaseContext) : ILogger
{

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        try
        {
            var message = state as RabbitMqMessageBase<RecipModel>;

            var serializedPayload = JsonSerializer.Serialize(message?.Payload);

            var log = new LogModel
            {
                Level = logLevel.ToString(),
                Message = message.RoutingKey,
                Payload = serializedPayload,
                ApplicationName = message.ApplicationName,
                Timestamp = message.Timestamp
            };

            _databaseContext.Logs.Add(log);

            _databaseContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Log insertion problem");
            throw;
        }
    }
}
