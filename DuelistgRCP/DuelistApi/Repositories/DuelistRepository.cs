using MongoDB.Driver;
using DuelistApi.Infrastructure.Documents;
using DuelistApi.Mappers;
using DuelistApi.Models;

namespace DuelistApi.Repositories;

public class DuelistRepository : IDuelistRepository
{
    private readonly IMongoCollection<DuelistDocument> _duelistCollection;

    public DuelistRepository(IMongoDatabase database)
    {
        _duelistCollection = database.GetCollection<DuelistDocument>("Duelists");
    }

    public async Task<Duelist?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var doc = await _duelistCollection.Find(d => d.Id == id).FirstOrDefaultAsync(cancellationToken);
        return doc?.ToDomain();
    }

    public async Task<Duelist> CreateAsync(Duelist duelist, CancellationToken cancellationToken)
    {
        var doc = duelist.ToDocument();
        await _duelistCollection.InsertOneAsync(doc, cancellationToken: cancellationToken);
        return doc.ToDomain();
    }
}
