using DinoApi.Models;

namespace DinoApi.Repositories;

public interface IDinoRepo
{
    Task<Dino> CreateAsync(Dino dino, CancellationToken cancellationToken);  
}
