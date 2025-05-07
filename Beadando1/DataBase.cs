using System;
using System.Data.SQLLite;

public class Class1
{
    public static class DataBase
    {
        private static string dbFolder = "Database";
        private static string dbFile = "users.db";
        private static string dbPath = Path.Combine(dbFolder, dbFile);

        
        public static string ConnectionString => $"Data Source={dbPath};Version=3;";

        
        public static void CreateDataBase()
        {           
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);
          
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath); 
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                  
                    string createTableSql = @"
                        CREATE TABLE IF NOT EXISTS users (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            username TEXT NOT NULL UNIQUE,
                            password TEXT NOT NULL
                        );
                    ";

                    using (var cmd = new SQLiteCommand(createTableSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
