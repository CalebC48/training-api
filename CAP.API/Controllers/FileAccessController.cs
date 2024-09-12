using System.Threading.Tasks;
using CAP.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;


namespace CAP.API.Controllers;

// Any non-api route will be handled by this controller
[Route("{*url}")]
[ApiController]
public class FileAccessController : ControllerBase
{
    private readonly FileAccessService _fileAccessService;

    public FileAccessController(DbContext context)
    {
        _fileAccessService = new FileAccessService(context);
    }

    [HttpGet]
    // [Authorize(Policy = "Unauthorized User")]
    public async Task<ActionResult> Get(string url)
    {
        // The purpose of this controller is to allow requests to files in the uploads folder,\
        // We no longer use wwwroot to store files, since we want to be able to control access to files
        // If the file doesn't exist, it will return a 404, and if the user doesn't have access
        // to the file, it will return a 403.
        // We also want to make sure the user is authenticated, so we can't just use the static files
        // Also, we need to set the cache and such to not cache to files, since multiple users might
        // have access to the same file, but with different permissions
        // var netId = User.Identity?.Name;
        var fileStream = await _fileAccessService.AttemptFindFile(url, "NET_ID");

        // Use provider to attempt to determine the mime type of the file being returned
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(url, out var contentType))
        {
            // If we don't know the mime type, set it to application/octet-stream, which is the default
            contentType = "application/octet-stream";
        }

        // Set the cache control to no-store, since we don't want to cache the file
        Response.Headers.Add("Cache-Control", "no-store no-cache must-revalidate post-check=0 pre-check=0");
        // Add other headers, since this is a secured file and it must be protected on each request
        Response.Headers.Add("Expires", "0");


        return File(fileStream, contentType, enableRangeProcessing: true);
    }
}