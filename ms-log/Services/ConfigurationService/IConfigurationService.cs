using ms_configuration.Ms_configuration.Models;
using ms_log.Models;

namespace ms_log.Services.ConfigurationService;

public interface IConfigurationService
{
    Task<MethodResult<RabbitMqConfigModel>> GetRabbitMqConfigAsync();
}
