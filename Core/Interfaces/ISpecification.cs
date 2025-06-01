using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }

        Expression<Func<T, object>>? OrderBy { get; }

        Expression<Func<T, object>>? OrderByDescending { get; }

        // Pagination properties
        int Take { get; }

        int Skip { get; }

        bool IsPagingEnabled { get; }

        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }
}
