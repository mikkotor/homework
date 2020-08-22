using Backend.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Backend.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(ILogger<DatabaseService> logger)
        {
            _logger = logger;

            _connectionString = new SqliteConnectionStringBuilder("Data Source=Backend.db;Cache=Shared")
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            PrepareDb();
        }

        private void PrepareDb()
        {
            try
            {
                using var connection = OpenNewConnection();
                var command = connection.CreateCommand();
                command.CommandText = GetSqlScript("CreateUsersTable.sql");
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Fatal error! Cannot prepare database for operation");
                throw;
            }
        }

        private DbConnection OpenNewConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private string GetSqlScript(string scriptName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var resource = assembly.GetManifestResourceStream(resourceNames.Single(x => x.Contains(scriptName)));

            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();
        }

        public User AddNewUser(string email, string passwordHash)
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
                command.CommandText = GetSqlScript("InsertNewUser.sql");

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
                while (reader.Read())
                {
                    newUser.Id = reader.GetInt32(0);
                    break;
                }
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add '{email}' to database.");
                return null;
            }
        }
    }
}
