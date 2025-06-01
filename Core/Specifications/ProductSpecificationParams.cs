namespace Core.Specifications
{
    public class ProductSpecificationParams
    {
        private const int MaxPageSize = 50;

        private List<string> _brands = [];

        private List<string> _types = [];

        private int _pageSize = 6;

        public List<string> Brands
        {
            get => _brands;
            set => _brands = NormalizeList(_brands);
        }

        public List<string> Types
        {
            get => _types;
            set => _types = NormalizeList(_types);
        }

        public string? Sort { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public static List<string> NormalizeList(List<string> input)
        {
            return [.. input.SelectMany(p => p.Split(',', StringSplitOptions.RemoveEmptyEntries))];
        }
    }
}
