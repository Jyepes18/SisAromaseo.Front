namespace NumbeSalud.Front.Models.Users;

public class UserPageResponse
{
    public IEnumerable<UserResponseDto> Data { get; set; } = [];
    public int Total { get; set; }
}