using System.ComponentModel.DataAnnotations;

namespace CoreLogic.Models.DTO.Auth;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}