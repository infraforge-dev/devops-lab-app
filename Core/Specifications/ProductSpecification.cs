using System.Linq.Expressions;
using Core.Entities;
using Core.Enums;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecificationParams specParams)
            : base(CreateProductFilter(specParams))
        {
            switch (specParams.Sort)
            {
                case nameof(ProductSortOptions.PriceAsc):
                    AddOrderBy(p => p.Price);
                    break;
                case nameof(ProductSortOptions.PriceDesc):
                    AddOrderByDescending(p => p.Price);
                    break;
                case nameof(ProductSortOptions.NameDesc):
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }

            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public static Expression<Func<Product, bool>> CreateProductFilter(ProductSpecificationParams specParams)
        {
            return p =>
                (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
                (!specParams.Brands.Any() || specParams.Brands.Contains(p.Brand)) &&
                (!specParams.Types.Any() || specParams.Types.Contains(p.Type));
        }
    }
}
