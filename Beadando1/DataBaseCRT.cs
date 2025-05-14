using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Beadando1
{
    public static class DataBase
    { //xd
        private const string DbFile = "users.db";
        private static readonly string ConnectionString = $"Data Source={DbFile}";

        public static void CreateDataBase()
        {
            if (!File.Exists(DbFile))
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var tableCommand = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    );";

                using var command = new SqliteCommand(tableCommand, connection);
                command.ExecuteNonQuery();
            }
        }

        public static bool RegisterUser(string username, string password, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                    INSERT INTO users (username, password)
                    VALUES (@username, @password);";
                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@password", password);

                insertCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 19)
                    errorMessage = "Ez a felhasználónév már létezik.";
                else
                    errorMessage = "Hiba történt a regisztráció során.";
                return false;
            }
        }

        public static bool LoginUser(string username, string password)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*) FROM users
                WHERE username = @username AND password = @password;";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
}
