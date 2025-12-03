using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using DinoApiRest.Infrastructure.Soap.Dtos;


namespace DinoApiRest.Infrastructure.Soap.Contracts;

[ServiceContract(Name = "DinoService", Namespace = "http://dino-api/dino-service")]
public interface IDinoContract
{
    [OperationContract]
    Task<DinoResponseDto> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken);
}
