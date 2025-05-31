using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T>
        where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<TResult?> GetEntityWithSpec<TResult>(IProjectedSpecification<T, TResult> spec);

        Task<IReadOnlyList<TResult>> ListAsync<TResult>(IProjectedSpecification<T, TResult> spec);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> SaveAllAsync();

        bool Exists(int id);

        Task<int> CountAsync(ISpecification<T> spec);
    }
}
