namespace NumbeSalud.Front.DTOs.User;

public class UserList
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Company { get; set; } = "";
    public string DisplayName => string.Join(" - ",
        new[] { Name, LastName, Company }
            .Where(x => !string.IsNullOrWhiteSpace(x)));
}
