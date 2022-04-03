using Backend.Helpers;
using Backend.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;

namespace Backend.Services;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(ILogger<DatabaseService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("Backend");
        PrepareDb();
    }

    /// <summary>
    /// Prepare database for operation, ie. create the Backend.db file and Users table within if not already present
    /// </summary>
    private void PrepareDb()
    {
        try
        {
            using var connection = OpenNewConnection();
            var command = connection.CreateCommand();
            command.CommandText = EmbeddedResourceHelper.GetFileContentsAsString("CreateUsersTable.sql");
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Fatal error! Cannot prepare database for operation");
            throw;
        }
    }

    /// <summary>
    /// Open a new connection to database using the configured connection string from appsettings.json
    /// </summary>
    /// <returns>A new open connection to Database.db</returns>
    private DbConnection OpenNewConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public User InsertNewUser(string email, string passwordHash)
    {
        var newUser = new User()
        {
            Email = email,
            PasswordHash = passwordHash
        };

        try
        {
            using var connection = OpenNewConnection();
            DbCommand command = connection.CreateCommand();
            command.CommandText = EmbeddedResourceHelper.GetFileContentsAsString("InsertNewUser.sql");

            var param1 = command.CreateParameter();
            param1.ParameterName = "Email";
            param1.Value = newUser.Email;
            param1.DbType = System.Data.DbType.String;
            command.Parameters.Add(param1);

            var param2 = command.CreateParameter();
            param2.ParameterName = "PasswordHash";
            param2.Value = newUser.PasswordHash;
            param2.DbType = System.Data.DbType.String;
            command.Parameters.Add(param2);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                newUser.Id = reader.GetInt32(0);
                return newUser;
            }
            else
                throw new Exception("Couldn't read primary key.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to add '{email}' to database.");
            return null;
        }
    }

    public User GetUserWithEmail(string email)
    {
        try
        {
            using var connection = OpenNewConnection();
            DbCommand command = connection.CreateCommand();
            command.CommandText = EmbeddedResourceHelper.GetFileContentsAsString("GetUserWithEmail.sql");

            var param1 = command.CreateParameter();
            param1.ParameterName = "Email";
            param1.Value = email;
            param1.DbType = System.Data.DbType.String;
            command.Parameters.Add(param1);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = new User
                {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    PasswordHash = reader.GetString(2)
                };
                return user;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to search for '{email}' from database.");
            return null;
        }
    }
}
