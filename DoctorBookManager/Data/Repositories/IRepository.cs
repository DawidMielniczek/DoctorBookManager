using DoctorBookManager.Data.EntityBase;

namespace DoctorBookManager.Data.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task InsertAsync(T entity);
        void Insert(T entity);
        Task<int> InsertAndGetIdAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllListAsync();
        IQueryable<T> GetAll();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(T entity);
    }
}
