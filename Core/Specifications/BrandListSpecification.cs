using Core.Entities;

namespace Core.Specifications
{
    public class BrandListSpecification : BaseSpecification<Product, string>
    {
        public BrandListSpecification()
        {
            AddSelector(p => p.Brand);
            ApplyDistinct();
        }
    }
}
