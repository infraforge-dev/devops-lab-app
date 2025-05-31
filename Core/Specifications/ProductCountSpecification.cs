using Core.Entities;

namespace Core.Specifications
{
    public class ProductCountSpecification(ProductSpecificationParams specParams) : BaseSpecification<Product>(ProductSpecification.CreateProductFilter(specParams))
    {
    }
}
