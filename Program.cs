using System;
using System.Configuration;

namespace activity_tracker
{
    class Program
    {
        static string connectionString = ConfigurationManager.AppSettings["ConnectionString"] ?? throw new InvalidOperationException("Connection string not found in app.config.");
        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            UserInput userInput = new();
            databaseManager.CreateTable(connectionString);
            userInput.MainMenu();
        }
    }
}
