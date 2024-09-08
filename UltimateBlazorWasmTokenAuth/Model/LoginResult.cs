namespace UltimateBlazorWasmTokenAuth.Model;

public class LoginResult
{
    public string? accessToken { get; set; }

    public string? refreshToken { get; set; }

    public DateTime Expiration { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(3);
}
