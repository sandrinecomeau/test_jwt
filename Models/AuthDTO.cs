// Models/AuthDtos.cs
public record LoginDto(string Username);

public class UserRecord
{
    public string Username { get; set; } = default!;
    public string Role     { get; set; } = default!;
}
