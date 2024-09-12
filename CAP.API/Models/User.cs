using CAP.API.Models.DTO;

namespace CAP.API.Models;

// Don't forget to add this to the DbContext class:
/*
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<WeatherForecast>().UseTimestampedProperty();
}
*/
// This adds the last modified, and created by fields to the database.

/// <summary>
/// An example User class
/// It inherits from the Timestamp class, which adds the CreatedTimestamp, UpdatedTimestamp fields within the Database
/// </summary>
public class User : TimeStamp
{
    /// <summary>
    ///  The User Records Id in the database
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// NetId of the user
    /// </summary>
    public string NetId { get; set; }

    /// <summary>
    ///  The user's first name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    ///  The user's last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///  The user's email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///  Default constructor
    /// </summary>
    public User()
    {
    }

    /// <summary>
    ///  Constructor that takes a UserDTO
    /// </summary>
    /// <param name="dto">
    /// The UserDTO to convert to a User
    /// </param>
    public User(UserDTO dto)
    {
        CopyFromDTO(dto);
    }

    public void CopyFromDTO(UserDTO dto)
    {
        // NEVER EVER EVER copy the Id from the dto into the User object
        // This is a security risk, and will allow users to modify other users
        NetId = dto.NetId;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Email = dto.Email;
    }
}