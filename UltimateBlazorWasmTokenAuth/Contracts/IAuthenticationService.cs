using UltimateBlazorWasmTokenAuth.Model;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface IAuthenticationService
{
    event Action<string?>? LoginChange;

    Task AnnounceLoginStatus();
    ValueTask<string> GetJwtAsync();
    Task<DateTime> LoginAsync(LoginCreds model);
    Task LogoutAsync();
    Task<bool> RefreshAsync();
}