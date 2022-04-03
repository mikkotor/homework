using Backend.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.IO;
using System.Reflection;
using Xunit;

namespace BackendIntegrationTests;

public class DatabaseServiceTests
{
    private readonly ILogger<DatabaseService> _logger;
    private readonly IConfiguration _configuration;
    private IDatabaseService _database;

    public DatabaseServiceTests()
    {
        _logger = Substitute.For<ILogger<DatabaseService>>();
        _configuration = Substitute.For<IConfiguration>();
        _configuration.GetConnectionString("Backend").Returns("Data Source=Test.db;Mode=ReadWriteCreate;Cache=Shared");
        PreTestCleanup();
    }

    [Fact]
    public void Test_Db_Creation_Insert_And_Select()
    {
        _database = new DatabaseService(_logger, _configuration);

        var result = _database.InsertNewUser("some@email.com", "foobar123");

        Assert.NotNull(result);
        Assert.Null(_database.InsertNewUser(result.Email, "bla"));
        Assert.NotNull(_database.GetUserWithEmail(result.Email));
    }

    private void PreTestCleanup()
    {
        var cwd = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
        var testDb = cwd.FullName + Path.DirectorySeparatorChar + "Test.db";
        if (File.Exists(testDb))
            File.Delete(testDb);
    }
}
