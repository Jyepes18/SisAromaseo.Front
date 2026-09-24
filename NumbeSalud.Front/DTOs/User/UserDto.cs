using System.ComponentModel.DataAnnotations;

namespace NumbeSalud.Front.DTOs.User;

public class UserDto
{
    
    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Name { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public int? TypeDocument { get; set; }
    public string? Document { get; set; }
}