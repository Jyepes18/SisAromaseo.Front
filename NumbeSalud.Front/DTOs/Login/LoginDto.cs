using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NumbeSalud.Front.DTOs.Login;

public class LoginDto
{
    [EmailAddress]
    [Required(ErrorMessage = "El campo Email es obligatorio")]
    public string Email { get; set; }
    [Required(ErrorMessage = "El campo contraseña es obligatorio")]
    public string Password { get; set; }
}