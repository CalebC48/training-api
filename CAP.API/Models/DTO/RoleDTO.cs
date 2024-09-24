using CAP.API.Extensions;
using CAP.API.Models;

namespace CAP.API.Models.DTO;


/// <summary>
/// A Data Transfer Object for the Role class
/// </summary>
public class RoleDTO
{
    /// <summary>
    ///  The User Records Id in the database
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///  The user's first name
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    ///  Default constructor
    /// </summary>
    public RoleDTO()
    {
    }

    public RoleDTO(Role role)
    {
        role.CopyPropertiesTo(this);
    }
}