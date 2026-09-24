namespace NumbeSalud.Front.Models.Users;

public class UserResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public int? TypeDocument { get; set; }
    public string? Document { get; set; }
}