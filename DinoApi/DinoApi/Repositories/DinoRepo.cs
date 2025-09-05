using DinoApi.Infrastructure;
using DinoApi.Mappers;
using DinoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DinoApi.Repositories;

public class DinoRepo : IDinoRepo
{
    private readonly DinoDbContext _context;

    public DinoRepo(DinoDbContext context)
    {
        _context = context;
    }

    public async Task<Dino> CreateAsync(Dino dino, CancellationToken cancellationToken)
    {
        bool exists = await _context.Dinos
        .AnyAsync(d => d.Nombre == dino.Nombre, cancellationToken);

        if (exists)
            throw new InvalidOperationException("Tu Dino Ya esta registrado previamente en la DinoPedia");

        var DinoToCreate = dino.ToEntity();
        DinoToCreate.Id = Guid.NewGuid();
        await _context.Dinos.AddAsync(DinoToCreate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return DinoToCreate.ToModel();

    }
    public async Task<Dino?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var dino = await _context.Dinos.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return dino.ToModel();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var dino = await _context.Dinos.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (dino == null)
            return false;

        _context.Dinos.Remove(dino);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<Dino?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}