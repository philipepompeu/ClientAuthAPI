
namespace ClientAuthAPI.Domain.Interfaces;

public interface IRepository<T>
{

    Task<T> CreateAsync(T entity);
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> UpdateAsync(string id, T entity);
    Task<bool> DeleteAsync(string id);

    /*TDocument ToDocument(T entity);
    T FromDocument(TDocument document);*/
}