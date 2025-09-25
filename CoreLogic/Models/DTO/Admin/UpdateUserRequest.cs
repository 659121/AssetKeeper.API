using System.ComponentModel.DataAnnotations;

namespace CoreLogic.Models.DTO.Admin;

public class UpdateUserRequest
{
    public bool? IsActive { get; set; } = null;
    public List<string>? Roles { get; set; } = null;
}
