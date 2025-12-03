using DuelistApi.Models;

namespace DuelistApi.Repositories;

public interface IDuelistRepository
{
    Task<Duelist?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Duelist> CreateAsync(Duelist duelist, CancellationToken cancellationToken);
}
