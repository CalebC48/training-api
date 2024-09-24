using System;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using CAP.API.Models;
using CAP.API.Models.DTO;
using CAP.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CAP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {

    private readonly TrainingContext _context;
    private readonly UserService _userService;

    public UsersController(TrainingContext context)
    {
        _context = context;
        _userService = new UserService(context);
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        return await _userService.GetAllUsers();
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult> PostUser(UserDTO userDTO)
    {
        try
        {
            await _userService.AddUser(userDTO);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDTO userDto)
    {
        try
        {
            await _userService.UpdateUser(id, userDto);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        
    }
}