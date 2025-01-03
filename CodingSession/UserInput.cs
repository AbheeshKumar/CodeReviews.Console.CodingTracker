using System;
using System.Globalization;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingSession
{
    internal class UserInput
    {

        internal void UserMenu()
        {
            var prompt = AnsiConsole.Prompt(
                    new SelectionPrompt<Enums.Options>()
                    .Title("Select desired Option")
                    .AddChoices(Enum.GetValues<Enums.Options>()));

            switch (prompt)
            {
                case Enums.Options.Insert:
                    InputData();
                    break;

                case Enums.Options.Display:
                    DisplayData();
                    break;
            }

        }


        internal void InputData()
        {
            var startTime = AnsiConsole.Ask<string>("[cyan]Enter when you started coding: [/] Hours:Min:Sec");
            var endTime = AnsiConsole.Ask<string>("[cyan]Enter when you ended coding: [/] Hours:Min:Sec");

            startTime = DateOnly.FromDateTime(DateTime.Now).ToString() + $" {startTime}";
            endTime = DateOnly.FromDateTime(DateTime.Now).ToString() + $" {endTime}";

            var start = DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-us"));
            var end = DateTime.ParseExact(endTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-us"));

            double duration = CalculateDuration(start, end);

            if (duration < 0)
            {
                AnsiConsole.MarkupLine("StartTime should not be greater than endTime");
                return;
            }
            AnsiConsole.MarkupLine($"Duration for the session is: {duration:n}");

            CodingController codingController = new CodingController();

            codingController.InsertQuery(start, end, duration);
        }

        internal void DisplayData()
        {
            CodingController controller = new();
            bool success = controller.DisplayQueryList();

            if (!success)
            {
                AnsiConsole.MarkupLine("Error Occurred");
                return;
            }

            var table = new Table();
            table.AddColumns("Id", "StartTime", "EndTime", "Duration");

            var sessions = CodingController.codingSessions;

            foreach (var session in sessions)
            {
                table.AddRow(session.Id.ToString(), session.StartTime.ToString("dd-MM-yyyy HH-mm-ss"), session.EndTime.ToString("dd-MM-yyyy HH-mm-ss"), session.Duration.ToString());
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to exit");
            Console.ReadKey();
        }
        internal double CalculateDuration(DateTime startTime, DateTime endTime)
        {
            double duration = endTime.Subtract(startTime).TotalHours;

            return Math.Round(duration, 2);
        }
    }
}
