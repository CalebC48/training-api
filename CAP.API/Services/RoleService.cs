using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using CAP.API.Models;
using CAP.API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Services;

/// <summary>
///  An example Role service
/// </summary>
public class RoleService : BaseService
{
    private readonly TrainingContext _context;
    /// <summary>
    ///  Create a new role service
    /// </summary>
    /// <param name="context">
    ///  The database context
    /// </param>
        public RoleService(TrainingContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<RoleDTO>> GetAllRoles()
    {
        var roles = await _context.Roles.Select(r => new RoleDTO(r)).ToListAsync();
        return roles;
    }
}