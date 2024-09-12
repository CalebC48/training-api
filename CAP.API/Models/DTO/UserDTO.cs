namespace CAP.API.Models.DTO;

/// <summary>
///  A Data Transfer Object for the User class
/// </summary>
public class UserDTO
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
    public UserDTO()
    {
    }

    /// <summary>
    ///  Constructor that takes a User
    /// </summary>
    /// <param name="user">
    /// The User to convert to a UserDTO
    /// </param>
    public UserDTO(User user)
    {
        CopyFromUser(user);
    }

    /// <summary>
    ///  Copy the values from a User object to a UserDTO
    /// </summary>
    /// <param name="user">
    /// The User to copy from
    /// </param>
    public void CopyFromUser(User user)
    {
        Id = user.Id;
        NetId = user.NetId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
    }
}