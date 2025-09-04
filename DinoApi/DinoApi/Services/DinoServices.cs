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
    }

}
