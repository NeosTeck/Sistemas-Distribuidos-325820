using DinoApiRest.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using DinoApiRest.Infrastructure.Soap.Dtos;
namespace DinoApiRest.Gateways;

public interface IDinoGateway
{
    Task<DinoResponseDto?> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken);
}
