using System;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using CAP.API.Models.DTO;
using CAP.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;

namespace CAP.API.Controllers;

public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(DbContext context)
    {
        _userService = new UserService(context);
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<UserDTO>> GetUser()
    {
        try
        {
            throw new Exception("Yeet");
        }
        // Catch all exceptions, if all else fails.
        catch (Exception e)
        {
            SentrySdk.CaptureException(e);
            return StatusCode(500, new { Yeet = "Yeet" });
        }
    }
}