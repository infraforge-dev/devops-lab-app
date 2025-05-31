using Core.Interfaces;

namespace Infrastructure.Data
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(
            IQueryable<T> query,
            ISpecification<T> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // No projection, just return the original query
            return query;
        }

        public static IQueryable<TResult> GetQuery<T, TResult>(
            IQueryable<T> query,
            IProjectedSpecification<T, TResult> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Handle projection and distinct
            if (spec.Selector is not null)
            {
                var projected = query.Select(spec.Selector);
                return spec.IsDistinct ? projected.Distinct() : projected;
            }
            else
            {
                var casted = query.Cast<TResult>();
                return spec.IsDistinct ? casted.Distinct() : casted;
            }
        }
    }
}
