using Microsoft.Extensions.Logging;
using ms_configuration.Ms_configuration.Models;
using ms_log.Constants;
using ms_log.Models;
using ms_log.Services.ConfigurationService;
using ms_recip.Ms_recip.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ms_log.Services.RabbitMq;

public class RabbitMqSubscriberService : IHostedService, IDisposable
{
    private readonly IConfigurationService _configurationService;
    private readonly Timer _timer;
    private IModel _channel;
    private IConnection _connection;
    private string _queueName;
    private readonly int _pollingInterval;
    private readonly IServiceProvider _serviceProvider;
    private RabbitMqConfigModel? _rabbitMqConfigModel;

    public RabbitMqSubscriberService(
        IConfigurationService configurationService,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _configurationService = configurationService;
        _pollingInterval = int.Parse(configuration["RabbitMQ:PollingInterval"] ?? "5000");

        _timer = new Timer(ProcessMessages, null, Timeout.Infinite, _pollingInterval);

        _serviceProvider = serviceProvider;

        _queueName = configuration["RabbitMqQueueName"];
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var rabbitMqConfigResult = await _configurationService.GetRabbitMqConfigAsync();

        if (!rabbitMqConfigResult.IsSuccess)
            return;

        _rabbitMqConfigModel = rabbitMqConfigResult.Value;

        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqConfigModel.Hostname,
            Port = _rabbitMqConfigModel.Port,
            UserName = _rabbitMqConfigModel.Username,
            Password = _rabbitMqConfigModel.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _timer.Change(0, _pollingInterval);
    }

    private void ProcessMessages(object state)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message from {_queueName}: {message}");

            if(ea.Exchange == RabbitmqConstants.RecipExchangeName
            && RecipActions.Contains(ea.RoutingKey))
                await HandleRecipAsync(message);
            
            if(ea.Exchange == RabbitmqConstants.RecipExchangeName
            && IngredientActions.Contains(ea.RoutingKey))
                await HandleIngredientAsync(message);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
    }

    private string[] RecipActions = 
    {
        RabbitmqConstants.CreateRecipResultRoutingKey,
        RabbitmqConstants.UpdateRecipResultRoutingKey,
        RabbitmqConstants.DeleteRecipResultRoutingKey
    };
    
    private string[] IngredientActions = 
    {
        RabbitmqConstants.CreateIngredientResultRoutingKey,
        RabbitmqConstants.UpdateIngredientResultRoutingKey,
        RabbitmqConstants.DeleteIngredientResultRoutingKey
    };

    private async Task HandleRecipAsync(string message)
    {
        await HandleAsync<RecipModel>(message);
    }
    
    private async Task HandleIngredientAsync(string message)
    {
        await HandleAsync<IngredientModel>(message);
    }
    
    private async Task HandleAsync<T>(string message)
    {
        try
        {
            var deserializedMessage = JsonSerializer.Deserialize<RabbitMqMessageBase<T>>(message);

            if (deserializedMessage != null)
            {
                using var scope = _serviceProvider.CreateScope();

                var logsService = scope.ServiceProvider.GetRequiredService<ILogger>();

                Func<RabbitMqMessageBase<T>, Exception?, string> func = null;

                logsService.Log(LogLevel.Information,
                                eventId: new EventId(),
                                state: deserializedMessage,
                                exception: null,
                                formatter: func);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du traitement du message: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        _channel?.Close();
        _connection?.Close();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
