using System.Collections;
using System.Threading.Tasks;
using CAP.API.Models;
using CAP.API.Models.DTO;
using CAP.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace CAP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase {

    private readonly TrainingContext _context;
    private readonly RoleService _roleService;

    public RolesController(TrainingContext context)
    {
        _context = context;
        _roleService = new RoleService(context);
    }

    // GET: api/Roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
    {
        return await _roleService.GetAllRoles();
    }
}