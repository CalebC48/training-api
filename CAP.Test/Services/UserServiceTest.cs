using CAP.Test.Models;
using Microsoft.EntityFrameworkCore;

namespace CAP.Test.Services;

// Replace all instances of DbContext with your db context
public class UserServiceTest
{
    private DbContext _context;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // This is run before any test in this class, we use the context builder to make sure we use an 
        // InMemory database, rather than a real database.
        // You use it the exact same way you would use a real database.
        var options = TestDbContextOptionsBuilder.GetOptions();
        _context = new DbContext(options);
        // Add anything you need to the InMemory database here
        //_context.SaveChanges after you add anything
    }

    [SetUp]
    public void Setup()
    {
        // Do your setup here
    }

    [TearDown]
    public void TearDown()
    {
        // Do your tear down here
    }

    [Test, Description("Explain What a Test is Doing")]
    public void Test1()
    {
        Assert.Pass();
    }
}