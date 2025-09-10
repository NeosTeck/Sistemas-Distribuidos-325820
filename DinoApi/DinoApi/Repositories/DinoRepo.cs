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

    // Implementación del método UpdateAsync
    public async Task<Dino?> UpdateAsync(Dino dino, CancellationToken cancellationToken)
{
    var existing = await _context.Dinos.FirstOrDefaultAsync(s => s.Id == dino.Id, cancellationToken);
    if (existing == null)
        return null;

    bool duplicate = await _context.Dinos
        .AnyAsync(d => d.Nombre == dino.Nombre && d.Id != dino.Id, cancellationToken);

    if (duplicate)
        throw new InvalidOperationException("Ya existe otro Dino con ese nombre");

    existing.Nombre = dino.Nombre;
    existing.Orden = dino.Orden;
    existing.Postura = dino.Postura;
    existing.PeriodoPpl = dino.PeriodoPpl;
    existing.Dieta = dino.Dieta;
    existing.Continente = dino.Continente;

    await _context.SaveChangesAsync(cancellationToken);

    return existing.ToModel();
}

public async Task<IEnumerable<Dino>> SearchAsync(
    string? orden = null,
    string? postura = null,
    string? periodoPpl = null,
    string? dieta = null,
    string? continente = null,
    CancellationToken cancellationToken = default)
{
    var query = _context.Dinos.AsQueryable();

    if (!string.IsNullOrWhiteSpace(orden))
        query = query.Where(d => d.Orden!.Contains(orden));

    if (!string.IsNullOrWhiteSpace(postura))
        query = query.Where(d => d.Postura!.Contains(postura));

    if (!string.IsNullOrWhiteSpace(periodoPpl))
        query = query.Where(d => d.PeriodoPpl!.Contains(periodoPpl));

    if (!string.IsNullOrWhiteSpace(dieta))
        query = query.Where(d => d.Dieta!.Contains(dieta));

    if (!string.IsNullOrWhiteSpace(continente))
        query = query.Where(d => d.Continente!.Contains(continente));

    var results = await query.AsNoTracking().ToListAsync(cancellationToken);
    return results.Select(r => r.ToModel());
}

}