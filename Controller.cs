using System.Configuration;
using Microsoft.Data.Sqlite;

namespace activity_tracker
{
    /***********controls the database via direct interaction with dB objects***********/
    internal class Controller
    {
        readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"] ?? throw new InvalidOperationException("Connection string not found in app.config.");
        internal void Get()
        {
            List<Tracker> trackerData = [];
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = "SELECT * FROM Activity";
                    cmd.ExecuteNonQuery();

                    using var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            trackerData.Add(
                                new Tracker
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Duration = reader.GetString(2)
                                }
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo rows of data found in table.\n");
                    }
                }  
            }
            TableVisualizer.ShowTable(trackerData);
        }

        internal Tracker GetById(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            using var cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandText = $"SELECT * FROM Activity WHERE Id = '{id}'";

            using var reader = cmd.ExecuteReader();
            Tracker tracker = new();
            // reader is reading in each field in the resulting query output row
            if (reader.HasRows)
            { // transferring the read values to Tracker object
                reader.Read();
                tracker.Id = reader.GetInt32(0);
                tracker.Date = reader.GetString(1);
                tracker.Duration = reader.GetString(2);
            }

            Console.WriteLine($"\nRecord '{tracker.Id}' has been selected for removal.\n");
            return tracker;
        }

        internal void Post(Tracker tracker)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"INSERT INTO Activity (Date, Duration) VALUES ('{tracker.Date}', '{tracker.Duration}')";
                    cmd.ExecuteNonQuery();
                }  
            }
        }

        internal void Update(Tracker tracker)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"UPDATE Activity SET
                                            Date = '{tracker.Date}',
                                            Duration = '{tracker.Duration}'
                                        WHERE Id = '{tracker.Id}'";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Record '{tracker.Id}' has been updated.\n");
                }  
            }
        }

        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"DELETE FROM Activity WHERE Id = '{id}'";
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"Record '{id}' has been deleted.\n");
                }  
            }
        }
    }
}