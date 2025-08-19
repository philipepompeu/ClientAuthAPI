
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;
public abstract class RepositoryBase<T> : IRepository<T>
{
    protected readonly IMongoCollection<T> _collection;

    protected RepositoryBase(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task<bool> UpdateAsync(string id, T entity)
    {
        var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}