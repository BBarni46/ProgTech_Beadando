using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace Beadando1
{
    public static class DataBase
    { //xd
      // Állítsd be saját MySQL adataidnak megfelelően
        private const string Server = "localhost";
        private const string DatabaseName = "users";
        private const string User = "root";
        private const string Password = ""; // vagy amit megadtál
        private static readonly string ConnectionString = $"Server={Server};Database={DatabaseName};Uid={User};Pwd={Password};";

        public static bool RegisterUser(string username, string password, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                connection.Open();

                var insertCommand = new MySqlCommand(
                    "INSERT INTO felhasználók (név, jelszó) VALUES (@név, @jelszó);", connection);
                insertCommand.Parameters.AddWithValue("@név", username);
                insertCommand.Parameters.AddWithValue("@jelszó", password);
                insertCommand.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // UNIQUE constraint violation
                    errorMessage = "Ez a felhasználónév már létezik.";
                else
                    errorMessage = $"Hiba történt: {ex.Message}";
                return false;
            }
        }

        public static bool LoginUser(string username, string password)
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var command = new MySqlCommand(
                "SELECT COUNT(*) FROM felhasználók WHERE név = @név AND jelszó = @jelszó;", connection);
            command.Parameters.AddWithValue("@név", username);
            command.Parameters.AddWithValue("@jelszó", password);

            long count = (long)(long?)command.ExecuteScalar();
            return count > 0;
        }
    }
}