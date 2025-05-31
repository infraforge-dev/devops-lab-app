using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications
{
    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecification<T>(criteria), IProjectedSpecification<T, TResult>
    {
        protected BaseSpecification()
            : this(null) { }

        public Expression<Func<T, TResult>>? Selector { get; private set; }

        public bool IsDistinct { get; private set; }

        protected void AddSelector(Expression<Func<T, TResult>> selectorExpression)
        {
            Selector = selectorExpression;
        }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
    }
}
