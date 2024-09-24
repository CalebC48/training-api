using CAP.Test.Models;
using Microsoft.EntityFrameworkCore;
using CAP.API.Models;
using CAP.API.Services;
using System.Threading.Tasks;

namespace CAP.Test.Services;

// Replace all instances of DbContext with your db context
public class UserServiceTest
{
    private TrainingContext _context;
    private UserService _userService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // This is run before any test in this class, we use the context builder to make sure we use an 
        // InMemory database, rather than a real database.
        // You use it the exact same way you would use a real database.
        var options = TestDbContextOptionsBuilder.GetOptions();
        _context = new TrainingContext(options);
        _userService = new UserService(_context);
        // Add anything you need to the InMemory database here
        //_context.SaveChanges after you add anything
    }

    [SetUp]
    public void Setup()
    {
        var user = new User()
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            Email = "email",
            NetId = "netId",
        };
        _context.Users.Add(user);

        var user2 = new User()
        {
            Id = 2,
            FirstName = "Test2",
            LastName = "User2",
            Email = "email2",
            NetId = "netId2",
        };
        _context.Users.Add(user2);

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        //remove all users
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
    }

    [Test, Description("Explain What a Test is Doing")]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test, Description("Gets all users in the DB")]
    public async Task GetAllUsersTest()
    {
        var users = await _userService.GetAllUsers();
        Assert.AreEqual(2, users.Count);
    }
}