using System.ComponentModel.DataAnnotations;

namespace FirstApi.DTOs;

public class UpdateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}