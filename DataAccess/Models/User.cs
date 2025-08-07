﻿namespace DataAccess.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime RegDate { get; set; }
    public DateTime? LastLogin {  get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}
