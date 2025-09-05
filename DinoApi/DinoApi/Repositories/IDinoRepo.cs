using DinoApi.Models;

namespace DinoApi.Repositories;

public interface IDinoRepo
{
    Task<Dino> CreateAsync(Dino dino, CancellationToken cancellationToken);

    Task<Dino?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
