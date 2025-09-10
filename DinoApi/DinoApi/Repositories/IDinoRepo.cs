using DinoApi.Models;

namespace DinoApi.Repositories;

public interface IDinoRepo
{
    Task<Dino> CreateAsync(Dino dino, CancellationToken cancellationToken);

    Task<Dino?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<Dino?> UpdateAsync(Dino dino, CancellationToken cancellationToken);
    Task<IEnumerable<Dino>> SearchAsync(string? orden, string? postura, string? periodoPpl, string? dieta, string? continente, CancellationToken cancellationToken);
}
