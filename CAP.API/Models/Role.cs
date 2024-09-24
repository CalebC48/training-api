using CAP.API.Models.DTO;

namespace CAP.API.Models;


/// <summary>
/// An example Role class
/// </summary>
public class Role : TimeStamp
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
    public Role()
    {
    }
}