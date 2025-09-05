using DinoApi.Dtos;
using DinoApi.Mappers;
using DinoApi.Repositories;
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
                throw new KeyNotFoundException("Dino not found");

            return dino.ToResponseDto();
        }

        public async Task<DeleteDinoResponseDto> DeleteDino(Guid id, CancellationToken cancellationToken)
        {
            bool success = await _repo.DeleteAsync(id, cancellationToken);
            return new DeleteDinoResponseDto { Success = success };
        }
    }

}
