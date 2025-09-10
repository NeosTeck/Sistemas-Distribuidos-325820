using System.ServiceModel;
using DinoApi.Dtos;
namespace DinoApi.Services
{
    [ServiceContract(Name = "DinoService", Namespace = "http://dino-api/dino-service")]
    public interface IDinoService
    {
        [OperationContract]
        Task<DinoResponseDto> CreateDino(CreateDinoDto dino, CancellationToken cancellationToken);

        [OperationContract]
        Task<DinoResponseDto> GetDinoById(Guid id, CancellationToken cancellationToken);

        [OperationContract]
        Task<DeleteDinoResponseDto> DeleteDino(Guid id, CancellationToken cancellationToken);

        [OperationContract]
        Task<DinoResponseDto> UpdateDino(UpdateDinoDto dino, CancellationToken cancellationToken);

        [OperationContract]
        Task<IEnumerable<DinoResponseDto>> SearchDinos(string? orden,string? postura,string? periodoPpl,string? dieta,string? continente, CancellationToken cancellationToken);

    }
}
