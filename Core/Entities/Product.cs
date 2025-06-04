using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Product : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = default!;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(2048)]
    public string PictureUrl { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string Brand { get; set; } = default!;

    [Range(0, 100000)]
    public int QuantityInStock { get; set; }
}
