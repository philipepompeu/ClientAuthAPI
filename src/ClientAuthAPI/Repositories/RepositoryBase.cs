
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;
public abstract class RepositoryBase<T, TDocument> : IRepository<T, TDocument>
{
    protected readonly IMongoCollection<TDocument> _collection;

    protected RepositoryBase(IMongoCollection<TDocument> collection)
    {
        _collection = collection;
    }

    public async Task<T> CreateAsync(T entity)
    {

        var document = ToDocument(entity);
        
        await _collection.InsertOneAsync(document);
        entity = FromDocument(document);
        return entity;
    }

    protected abstract TDocument ToDocument(T? entity);
    public abstract T FromDocument(TDocument document);

    public async Task<T?> GetByIdAsync(string id)
    {
        TDocument document = await _collection.Find(Builders<TDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync();

        return FromDocument(document);        
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync()
            .ContinueWith(task => task.Result.Select(FromDocument));
    }

    public async Task<bool> UpdateAsync(string id, T entity)
    {
        var document = ToDocument(entity);

        var result = await _collection.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", id), document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", id));
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    TDocument IRepository<T, TDocument>.ToDocument(T entity)
    {
        return ToDocument(entity);
    }

    
    
}