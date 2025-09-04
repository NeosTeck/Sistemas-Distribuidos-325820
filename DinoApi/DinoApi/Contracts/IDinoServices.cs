using System.ServiceModel;
using DinoApi.Dtos;
namespace DinoApi.Services
{
    [ServiceContract(Name = "DinoService", Namespace = "http://dino-api/dino-service")]
    public interface IDinoService
    {
        [OperationContract]
        Task<DinoResponseDto> CreateDino(CreateDinoDto dino, CancellationToken cancellationToken);
    }
}
