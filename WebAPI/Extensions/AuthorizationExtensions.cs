using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static AuthorizationOptions AddCustomPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy("RequireAdminRole", policy => 
            policy.RequireRole("Admin"));
        options.AddPolicy("RequireUserRole", policy => 
            policy.RequireRole("User"));
        return options;
    }
}