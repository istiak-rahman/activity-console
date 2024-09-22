using Microsoft.Data.Sqlite;

namespace activity_tracker
{
    internal class DatabaseManager
    {
        internal void CreateTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            { // sql command is a managed resource that needs to be disposed of -- hence, wrapped in using statement
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = 
                        @"CREATE TABLE IF NOT EXISTS Activity (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Duration TEXT
                        )";

                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("db has been updated\n");
            }
        }
    }
}