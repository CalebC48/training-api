using System.IO;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace CAP.API.Services;

public class FileAccessService : BaseService
{
    public FileAccessService(DbContext context) : base(context)
    {
    }

    /// <summary>
    ///   Attempt to find a file by the given path and validate netId can access resource
    ///   This is relative to the uploads folder, not wwwroot, as wwwroot will allow clear access to all files
    ///   So if any user can access the file, wwwroot is fine, but if you need granular access, use uploads, and this service
    /// </summary>
    /// <param name="netId">
    ///  The netId of the user attempting to access the file
    /// </param>
    /// <param name="path">
    /// The path to the file
    /// </param>
    /// <returns>
    /// A stream of the file
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the file is not found
    /// </exception>
    public async Task<Stream> AttemptFindFile(string path, string netId)
    {
        if (netId is null)
        {
            throw new NotFoundException();
        }

        // Throw an error if there are any invalid file name characters
        if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new NotFoundException();
        }

        // var user = await _context.Users.FirstOrDefaultAsync(u => u.NetId == netId);
        // var student = await _context.Students.FirstOrDefaultAsync(s => s.NetId == netId);
        //
        // if (user is null && student is null)
        // {
        //     throw new NotFoundException();
        // }

        // Prevent path traversal attacks
        if (path is null)
        {
            throw new NotFoundException();
        }

        var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "uploads", path));

        if (!filePath.StartsWith(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "uploads"))))
        {
            throw new NotFoundException();
        }

        if (!File.Exists(filePath))
        {
            throw new NotFoundException();
        }

        // if (user is not null)
        // {
        //     // Role 1 == Admin
        //     if (user.RoleId != 1)
        //     {
        //         throw new ForbiddenException();
        //     }
        // }
        // else
        // {
        //     // Make sure the photoUrl on the student object is the same as the requested path
        //     if (student.PhotoUrl != path)
        //     {
        //         throw new ForbiddenException();
        //     }
        // }

        return File.OpenRead(filePath);
    }
}