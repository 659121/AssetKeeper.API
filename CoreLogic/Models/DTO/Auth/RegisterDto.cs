﻿using System.ComponentModel.DataAnnotations;

namespace CoreLogic.Models.DTO.Auth;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;
}
