using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Services.Auth;

public class AuthorizationService
{
    // Replace DbContext with your db context
    protected readonly DbContext _context;

    public AuthorizationService(DbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Example authorization method
    /// </summary>
    /// <param name="user">The ClaimsPrincipal user</param>
    /// <param name="netId">The user netId to check against</param>
    /// <returns>AuthorizationResult.Success() if the user has access, AuthorizationResult.Failure() if not</returns>
    public Task<AuthorizationResult> UserOwnsApplicationByNetId(ClaimsPrincipal user, string netId)
    {
        /*var requestingUser = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.NetId == user.Identity.Name);
        if (requestingUser == null)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }

        //only grant non-admin users applications that match their netid
        if (requestingUser.Role.RoleName != "Admin" && netId != user.Identity.Name)
        {
            return Task.FromResult(AuthorizationResult.Failed());
        }*/

        return Task.FromResult(AuthorizationResult.Success());
    }
}