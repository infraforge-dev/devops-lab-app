using Core.Entities;
using Core.Enums;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? brand, string? type, string? sort)
            : base(p =>
            (string.IsNullOrWhiteSpace(brand) || p.Brand == brand) &&
            (string.IsNullOrWhiteSpace(type) || p.Type == type))
        {
            switch (sort)
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
        }
    }
}
