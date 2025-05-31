using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IProjectedSpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }

        bool IsDistinct { get; }
    }
}
