using System.ComponentModel.DataAnnotations;

namespace UltimateBlazorWasmTokenAuth.Model;

public class LoginCreds
{
    [Required]
    public string? username { get; set; }

    [Required]
    public string? password { get; set; }
}
