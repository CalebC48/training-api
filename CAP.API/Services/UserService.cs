using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using CAP.API.Models;
using CAP.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Services;

/// <summary>
///  An example user service
/// </summary>
public class UserService : BaseService
{
    private readonly TrainingContext _context;
    /// <summary>
    ///  Create a new user service
    /// </summary>
    /// <param name="context">
    /// The database context
    /// </param>
    public UserService(TrainingContext context) : base(context)
    {
        _context = context;
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

    public async Task<List<UserDTO>> GetAllUsers()
    {
        return await _context.Users.Select(u => new UserDTO(u)).ToListAsync();
    }

    public async Task AddUser(UserDTO userDTO)
    {
        var user = new User(userDTO);
        if (user is null)
        {
            throw new NotFoundException();
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(int id, UserDTO userDto)
    {
        var userToEditInDb = await _context.Users.FindAsync(id);
        if (userToEditInDb is null)
        {
            throw new NotFoundException();
        }

        userToEditInDb.CopyFromDTO(userDto);

        _context.Entry(userToEditInDb).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            throw new NotFoundException();
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}