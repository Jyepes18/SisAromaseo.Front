using System.ComponentModel.DataAnnotations;

namespace NumbeSalud.Front.DTOs.Product;

public class ProductoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La presentación es obligatoria")]
    public string Presentation { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    public decimal? Price { get; set; }
}