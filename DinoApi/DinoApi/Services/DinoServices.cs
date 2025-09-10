using DinoApi.Dtos;
using DinoApi.Mappers;
using DinoApi.Repositories;
using DinoApi.Models;
using DinoApi.Services;
namespace DinoApi.Services
{
    public class DinoService : IDinoService
    {
        private readonly IDinoRepo _repo;

        public DinoService(IDinoRepo repo)
        {
            _repo = repo;
        }

        public async Task<DinoResponseDto> CreateDino(CreateDinoDto dino, CancellationToken ct)
        {
            var model = dino.ToModel();
            var created = await _repo.CreateAsync(model, ct);
            return created.ToResponseDto();
        }

        public async Task<DinoResponseDto> GetDinoById(Guid id, CancellationToken cancellationToken)
        {
            var dino = await _repo.GetByIdAsync(id, cancellationToken);
            if (dino == null)
                throw new KeyNotFoundException("Dino no encontrado");

            return dino.ToResponseDto();
        }

        public async Task<DeleteDinoResponseDto> DeleteDino(Guid id, CancellationToken cancellationToken)
        {
            bool success = await _repo.DeleteAsync(id, cancellationToken);
            return new DeleteDinoResponseDto { Success = success };
        }

        public async Task<DinoResponseDto> UpdateDino(UpdateDinoDto dto, CancellationToken ct)
        {
            var model = dto.ToModel();
            var updated = await _repo.UpdateAsync(model, ct);

            if (updated == null)
                throw new KeyNotFoundException("El Dino aun no se descubre");

            return updated.ToResponseDto();
        }

        public async Task<IEnumerable<DinoResponseDto>> SearchDinos(string? orden,string? postura,string? periodoPpl,string? dieta,string? continente,CancellationToken ct)
        {
            var results = await _repo.SearchAsync(orden, postura, periodoPpl, dieta, continente, ct);

            if (!results.Any())
                throw new KeyNotFoundException("No se encontraron Dinosaurios, siguen sin descubrirse");

            return results.Select(r => r.ToResponseDto());
        }

        }

}
