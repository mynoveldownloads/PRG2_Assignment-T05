using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PRG2_T05_CFFTFlight;
using PRG2_T05_DDJBFlight;
using PRG2_T05_Flight;
using PRG2_T05_LWTTFlight;
using PRG2_T05_NORMFlight;

namespace PRG2_T05_Flight
{
    internal class Program
    {
        private static Terminal? _terminal;

        static void Main(string[] args)
        {
            _terminal = new Terminal("Terminal 5");
            InitializeSystem();
            DisplayMainMenu();
        }

        static void InitializeSystem()
        {
            LoadAirlines();
            LoadBoardingGates();
            LoadFlights();
        }

        static void LoadAirlines()
        {
            try
            {
                using var reader = new StreamReader("airlines.csv");
                while (!reader.EndOfStream)
                {
                    var data = reader.ReadLine()?.Split(',');
                    if (data?.Length >= 2)
                    {
                        var airline = new Airline(
                            data[0].Trim(),
                            data[1].Trim()
                        );
                        _terminal?.AddAirline(airline);
                    }
                }
                Console.WriteLine("Airlines loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

        static void LoadBoardingGates()
        {
            try
            {
                using var reader = new StreamReader("boardinggates.csv");
                while (!reader.EndOfStream)
                {
                    var data = reader.ReadLine()?.Split(',');
                    if (data?.Length >= 4)
                    {
                        var gate = new BoardingGate(
                            data[0].Trim(),
                            bool.Parse(data[1].Trim()),
                            bool.Parse(data[2].Trim()),
                            bool.Parse(data[3].Trim())
                        );
                        _terminal?.AddBoardingGate(gate);
                    }
                }
                Console.WriteLine("Boarding gates loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

        static void LoadFlights()
        {
            try
            {
                var lines = File.ReadAllLines("flights.csv");
                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(',');
                    if (data.Length < 5) continue;

                    var flight = CreateFlight(
                        data[0].Trim(),
                        data[1].Trim(),
                        data[2].Trim(),
                        DateTime.Parse(data[3].Trim()),
                        data[4].Trim()
                    );

                    if (flight != null && _terminal != null)
                    {
                        _terminal.Flights.TryAdd(flight.FlightNumber, flight);
                        var airline = _terminal.GetAirlineFromFlight(flight);
                        airline?.AddFlight(flight);
                    }
                }
                Console.WriteLine("Flights loaded successfully.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading flights: {ex.Message}");
            }
        }

        static Flight CreateFlight(string number, string origin, string dest, DateTime time, string requestCode)
        {
            return requestCode switch
            {
                "LWTT" => new LWTTFlight(number, origin, dest, time),
                "DDJB" => new DDJBFlight(number, origin, dest, time),
                "CFFT" => new CFFTFlight(number, origin, dest, time),
                _ => new NORMFlight(number, origin, dest, time)
            };
        }

        static void DisplayMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Changi Airport Terminal 5 ===");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("3. Assign Boarding Gate to Flight");
                Console.WriteLine("4. Create New Flight");
                Console.WriteLine("5. Display Airline Flights");
                Console.WriteLine("6. Modify Flight Details");
                Console.WriteLine("7. Display Flight Schedule");
                Console.WriteLine("8. Calculate Airline Fees");
                Console.WriteLine("0. Exit");

                var choice = GetValidIntegerInput("Enter your choice: ", 0, 8);

                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Exiting system...");
                        return;
                    case 1:
                        ListAllFlights();
                        break;
                    case 2:
                        ListBoardingGates();
                        break;
                    case 3:
                        AssignBoardingGate();
                        break;
                    case 4:
                        CreateNewFlight();
                        break;
                    case 5:
                        DisplayAirlineFlights();
                        break;
                    case 6:
                        ModifyFlightDetails();
                        break;
                    case 7:
                        DisplayFlightSchedule();
                        break;
                    case 8:
                        CalculateAirlineFees();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        static void ListAllFlights()
        {
            if (_terminal?.Flights == null || !_terminal.Flights.Any())
            {
                Console.WriteLine("No flights available.");
                return;
            }

            Console.WriteLine("\n{0,-15} {1,-20} {2,-15} {3,-15} {4,-25} {5,-10}",
                "Flight No.", "Airline", "Origin", "Destination", "Expected Time", "Status");

            foreach (var flight in _terminal.Flights.Values)
            {
                var airline = _terminal.GetAirlineFromFlight(flight);
                Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15} {4,-25} {5,-10}",
                    flight.FlightNumber,
                    airline?.Name ?? "Unknown",
                    flight.Origin,
                    flight.Destination,
                    flight.ExpectedTime.ToString("g"),
                    flight.Status);
            }
        }

        static void CreateNewFlight()
        {
            var flightNumber = GetValidInput("Enter flight number (e.g., SQ123): ",
                s => s.Length >= 4 && s.Any(char.IsLetter) && s.Any(char.IsDigit));

            var origin = GetValidInput("Enter origin: ", s => !string.IsNullOrWhiteSpace(s));
            var destination = GetValidInput("Enter destination: ", s => !string.IsNullOrWhiteSpace(s));

            var expectedTime = GetValidDateTimeInput("Enter expected time (dd/MM/yyyy HH:mm): ");
            var requestCode = GetValidInput("Enter special request code (CFFT/DDJB/LWTT/None): ",
                s => string.IsNullOrEmpty(s) || new[] { "CFFT", "DDJB", "LWTT", "NONE" }.Contains(s.ToUpper()));

            try
            {
                var flight = CreateFlight(
                    flightNumber,
                    origin,
                    destination,
                    expectedTime,
                    requestCode.ToUpper()
                );

                if (_terminal != null && flight != null)
                {
                    // Add to terminal
                    _terminal.Flights.TryAdd(flight.FlightNumber, flight);

                    // Add to airline
                    var airline = _terminal.GetAirlineFromFlight(flight);
                    airline?.AddFlight(flight);

                    // Append to CSV
                    File.AppendAllText("flights.csv",
                        $"\n{flightNumber},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{requestCode}");

                    Console.WriteLine($"Flight {flightNumber} created successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating flight: {ex.Message}");
            }
        }

        #region Helper Methods
        static int GetValidIntegerInput(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;
                Console.WriteLine($"Invalid input. Please enter a number between {min}-{max}.");
            }
        }

        static DateTime GetValidDateTimeInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParseExact(Console.ReadLine(),
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime result))
                    return result;
                Console.WriteLine("Invalid date format. Use dd/MM/yyyy HH:mm format.");
            }
        }

        static string GetValidInput(string prompt, Func<string, bool> validation)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim() ?? "";
                if (validation(input)) return input;
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
        #endregion

        #region Placeholder Methods for Future Implementation
        static void ListBoardingGates()
        {
            Console.WriteLine("\nFeature under development...");
        }

        static void AssignBoardingGate()
        {
            Console.WriteLine("\nFeature under development...");
        }

        static void DisplayAirlineFlights()
        {
            Console.WriteLine("\nFeature under development...");
        }

        static void ModifyFlightDetails()
        {
            Console.WriteLine("\nFeature under development...");
        }

        static void DisplayFlightSchedule()
        {
            Console.WriteLine("\nFeature under development...");
        }

        static void CalculateAirlineFees()
        {
            Console.WriteLine("\nFeature under development...");
        }
        #endregion
    }
}