using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Services;

/// <summary>
///  A base service class that can be used to inject the DbContext into any service
/// </summary>
public class BaseService
{
    // Set the context here, just specify the type of context you want to use.
    protected readonly DbContext _context;

    /// <summary>
    ///  Constructor that takes a DbContext
    /// </summary>
    /// <param name="context">
    /// The DbContext to use
    /// </param>
    public BaseService(DbContext context)
    {
        Debug.WriteLine("Warning: The context type has not been set.");
        Debug.WriteLine(
            "Please set the context type in the constructor of the BaseService class, remove this message.");
        _context = context;
    }
}