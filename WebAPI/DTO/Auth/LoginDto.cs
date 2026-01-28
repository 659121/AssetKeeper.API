using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO.Auth;
public class LoginDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}