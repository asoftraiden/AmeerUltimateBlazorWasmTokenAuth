using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.Helper;
using UltimateBlazorWasmTokenAuth.Model;

namespace UltimateBlazorWasmTokenAuth.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _factory;
    //private ISessionStorageService _sessionStorageService;
    private ILocalStorageService _sessionStorageService;
    private readonly TokenAuthenticationStateProvider _authStateProvider;
    private const string JWT_KEY = nameof(JWT_KEY);
    private const string REFRESH_KEY = nameof(REFRESH_KEY);
    //private const string EXP_KEY = nameof(EXP_KEY);

    private string? _jwtCache;

    public event Action<string?>? LoginChange;

    public AuthenticationService(IHttpClientFactory factory, ILocalStorageService sessionStorageService, TokenAuthenticationStateProvider authStateProvider)
    {
        _factory = factory;
        _sessionStorageService = sessionStorageService;
        _authStateProvider = authStateProvider;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        if (string.IsNullOrEmpty(_jwtCache))
            _jwtCache = await _sessionStorageService.GetItemAsync<string>(JWT_KEY);

        return _jwtCache;
    }

    public async Task LogoutAsync()
    {
        //var response = await _factory.CreateClient("ServerApi").DeleteAsync("api/authentication/revoke");

        await _sessionStorageService.RemoveItemAsync(JWT_KEY);
        await _sessionStorageService.RemoveItemAsync(REFRESH_KEY);
        //await _sessionStorageService.RemoveItemAsync(EXP_KEY);

        _jwtCache = null;

        // await Console.Out.WriteLineAsync($"Revoke gave response {response.StatusCode}");

        LoginChange?.Invoke(null);
    }

    public async Task AnnounceLoginStatus()
    {

        var AccessToken = await _sessionStorageService.GetItemAsync<string>(JWT_KEY);
        //var RefreshToken = await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY);
        //var Expiration = await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY);

        if (AccessToken is not null)
            LoginChange?.Invoke(GetUsername(AccessToken));
        else
            LoginChange?.Invoke(null);

    }






    private static string GetUsername(string token)
    {
        var jwt = new JwtSecurityToken(token);

        return jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;
    }

    public async Task<DateTime> LoginAsync(Login model)
    {

        var jsonContent = JsonContent.Create(model);
        var client = _factory.CreateClient("ServerApi");

        var response = await client.PostAsync("api/authentication/login", jsonContent);


        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Login failed.");

        var content = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (content == null)
            throw new InvalidDataException();

        await _sessionStorageService.SetItemAsync(JWT_KEY, content.accessToken);
        await _sessionStorageService.SetItemAsync(REFRESH_KEY, content.refreshToken);
        //await _sessionStorageService.SetItemAsync(EXP_KEY, content.Expiration);
        await _authStateProvider.SetTokenAsync(content.accessToken, content.Expiration);
        LoginChange?.Invoke(GetUsername(content.accessToken));

        return content.Expiration;
    }

    public async Task<bool> RefreshAsync()
    {
        var model = new RefreshModel
        {
            AccessToken = await _sessionStorageService.GetItemAsync<string>(JWT_KEY),
            RefreshToken = await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY)
        };

        var response = await _factory.CreateClient("ServerApi").PostAsync("api/token/refresh",
                                                    JsonContent.Create(model));

        if (!response.IsSuccessStatusCode)
        {
            await LogoutAsync();

            return false;
        }

        var content = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (content == null)
            throw new InvalidDataException();

        await _sessionStorageService.SetItemAsync(JWT_KEY, content.accessToken);
        await _sessionStorageService.SetItemAsync(REFRESH_KEY, content.refreshToken);
        //await _sessionStorageService.SetItemAsync(EXP_KEY, content.Expiration);

        _jwtCache = content.accessToken;

        return true;
    }

   
}