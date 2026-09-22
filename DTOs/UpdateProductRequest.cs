using System.ComponentModel.DataAnnotations;

namespace FirstApi.DTOs;

public class UpdateProductRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, 9999999)]
    public decimal Price { get; set; }
}