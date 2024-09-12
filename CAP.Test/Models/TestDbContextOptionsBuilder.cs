// Import your db context here

using CAP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CAP.Test.Models;

// Change all occurrences of DbContext to your db context
public static class TestDbContextOptionsBuilder
{
    public static DbContextOptions<DbContext> GetOptions()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        return options;
    }
}