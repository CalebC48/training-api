using System.Threading.Tasks;
using CAP.API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Services;

/// <summary>
///  An example user service
/// </summary>
public class UserService : BaseService
{
    /// <summary>
    ///  Create a new user service
    /// </summary>
    /// <param name="context">
    /// The database context
    /// </param>
    public UserService(DbContext context) : base(context)
    {
    }

    /// <summary>
    ///   Get a example user
    /// </summary>
    /// <returns>
    ///  A example user
    /// </returns>
    public async Task<UserDTO> GetUser()
    {
        // This is just a stub method, don't worry about it.
        // Go ahead and clear the code before you start.
        await Task.Delay(1);
        return new UserDTO();
    }
}