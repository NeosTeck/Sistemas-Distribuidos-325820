using System.ServiceModel;
using DinoApiRest.Infrastructure.Soap.Contracts;
using DinoApiRest.Infrastructure.Soap.Dtos;

namespace DinoApiRest.Gateways;

public class DinoGateway : IDinoGateway
{
    private readonly IDinoContract _dinoContract;
    private readonly ILogger<DinoGateway> _logger;

    public DinoGateway(IConfiguration configuration, ILogger<DinoGateway> logger)
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(configuration.GetValue<string>("DinoService:Url"));
        _dinoContract = new ChannelFactory<IDinoContract>(binding, endpoint).CreateChannel();
        _logger = logger;
    }

    public async Task<DinoResponseDto?> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dinoContract.GetDinoByIdAsync(id, cancellationToken);
        }
        catch (FaultException ex) when (ex.Message.Contains("Dino not found"))
        {
            _logger.LogWarning(ex, "Dino not found: {Id}", id);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling SOAP API for Dino {Id}", id);
            throw;
        }
    }
}
