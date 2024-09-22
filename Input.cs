using System.Globalization;
using System.Runtime.CompilerServices;

namespace activity_tracker
{
    internal class UserInput
    {
        Controller controller = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp is false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the application");
                Console.WriteLine("Type 1 to View a past session");
                Console.WriteLine("Type 2 to Record a new session");
                Console.WriteLine("Type 3 to Delete a session");
                Console.WriteLine("Type 4 to Update a session\n");

                string? input = Console.ReadLine();
                while (string.IsNullOrEmpty(input)) 
                {
                    Console.WriteLine("Please choose from menu options.\n");
                    input = Console.ReadLine();
                }

                switch (input)
                {
                    case "0":
                        closeApp = true; Environment.Exit(0); break;
                    case "1":
                        controller.Get();
                        break;
                    case "2":
                        AddNewRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid input\n");
                        break;
                }
            }
        }

        private void UpdateRecord()
        {
            controller.Get();
            Console.WriteLine("Please enter ID of record to be updated. Press 0 to return to Main Menu.\n");
            
            string? input = Console.ReadLine();
            while (!int.TryParse(input, out _) || string.IsNullOrEmpty(input) || int.Parse(input) < 0)
            {
                Console.WriteLine("\nInvalid ID selected.\n");
                input = Console.ReadLine();
            }

            var Id = int.Parse(input);
            if (Id == 0) MainMenu();

            var tracker = controller.GetById(Id);
            while (tracker.Id == 0)
            {
                Console.WriteLine($"\nRecord {Id} not found. Insert ID of record to be updated (or 0 for Main Menu).\n");
                UpdateRecord();
            }

            bool update = true;
            while (update == true)
            {
                Console.WriteLine($"\nWhich property would you like to update?\n");
                Console.WriteLine($"\nType 'd' for Date \n");
                Console.WriteLine($"\nType 't' for Duration \n");
                Console.WriteLine($"\nType 's' to save update \n");
                Console.WriteLine($"\nType '0' to Go Back to Main Menu \n");

                string? updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "d":
                        tracker.Date = GetDate();
                        break;

                    case "t":
                        tracker.Duration = GetDuration();
                        break;

                    case "0":
                        MainMenu();
                        update = false;
                        break;

                    case "s":
                        update = false;
                        break;

                    default:
                        Console.WriteLine($"\nType '0' to Go Back to Main Menu\n");
                        break;
                }
            }
            controller.Update(tracker);
            MainMenu();
        }

        private void DeleteRecord()
        {
            controller.Get();
            Console.WriteLine("Please enter ID of record to be deleted. Press 0 to return to Main Menu.\n");

            string? input = Console.ReadLine();
            while (!int.TryParse(input, out _) || string.IsNullOrEmpty(input) || int.Parse(input) < 0)
            {
                Console.WriteLine("\nInvalid ID selected.\n");
                input = Console.ReadLine();
            }

            var Id = int.Parse(input);
            if (Id == 0) MainMenu();

            var tracker = controller.GetById(Id);
            // check for existence of record (main point of using GetById() func)
            while (tracker.Id == 0)
            {
                Console.WriteLine($"\nRecord {Id} not found. Insert ID of record to be deleted (or 0 for Main Menu).\n");
                DeleteRecord();
            }

            controller.Delete(tracker.Id);
        }

        // perform process operations before data is sent to database
        private void AddNewRecord()
        {
            var date = GetDate();
            var duration = GetDuration();

            Tracker tracker = new()
            {
                Date = date,
                Duration = duration
            };

            controller.Post(tracker);
        }

        internal string GetDate()
        {
            Console.WriteLine("\n\nPlease insert the date (mm-dd-yyyy). Type 0 to return to Main Menu.\n\n");
            string? inputDate = Console.ReadLine();

            if (inputDate == "0")
                MainMenu();
            
            while (!DateTime.TryParseExact(inputDate, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine($"Invalid date: {inputDate}\nPlease enter date in following format: mm-dd-yyyy\n");
                inputDate = Console.ReadLine(); 
            }

            if (inputDate is null)
            {
                Console.WriteLine($"Invalid date: {inputDate}. Using default instead.");
                inputDate = "01-01-1999";
            }

            return inputDate;
        }

        internal string GetDuration()
        {
            Console.WriteLine("\n\nPlease insert the duration: (Format: hh:mm). Type 0 to return to Main Menu.\n\n");
            string? inputDuration = Console.ReadLine();

            if (inputDuration == "0")
                MainMenu();

            bool inValidTime = TimeSpan.TryParseExact(inputDuration, "h\\:mm", CultureInfo.InvariantCulture, out _);
            if (!inValidTime)
            {
                Console.WriteLine($"Invalid time span: {inputDuration}\nPlease enter date in following format: hh:mm\n");
                inputDuration = Console.ReadLine(); 
            }

#pragma warning disable CS8603 // Possible null reference return.
            return inputDuration;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}