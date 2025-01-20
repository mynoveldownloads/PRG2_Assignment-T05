using System;
using System.Collections.Generic;
using System.IO;
using PRG2_T05_CFFTFlight;
using PRG2_T05_DDJBFlight;
using PRG2_T05_Flight;
using PRG2_T05_LWTTFlight;
using PRG2_T05_NORMFlight;


namespace PRG2_T05_Flight
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // do not tamper
            Dictionary<string, Flight> flight_dict = new Dictionary<string, Flight>();
            Dictionary<string, Airline> airline_dict = new Dictionary<string, Airline>();
            Dictionary<string, BoardingGate> boarding_gate_dict = new Dictionary<string, BoardingGate>();
            LoadAirlinesCSV(airline_dict);
            LoadBoardingGatesCSV(boarding_gate_dict);
            DisplayMenu(flight_dict, airline_dict, boarding_gate_dict);

            //Dictionary<string, Flight> flight_list = new Dictionary<string, Flight>();

        }

        /// <summary>
        /// Loads airlines from a CSV file and adds them to a dictionary.
        /// This is Feature 1: Loading Airlines
        /// </summary>
        /// 

        // rphl edit: added airline dict as argument and add airline to dict
        static void LoadAirlinesCSV(Dictionary<string, Airline> airline_dict)
        {
            try
            {
                using (StreamReader reader = new StreamReader("airlines.csv"))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        if (data.Length >= 2)
                        {
                            string airlineCode = data[0].Trim();
                            string airlineName = data[1].Trim();
                            Airline airline = new Airline(airlineCode, airlineName);
                            airline_dict[airlineCode] = airline; // rphl edit: added every airline object to airline dict
                                                                 // airline_dict in main method is updated
                                                                 //Console.WriteLine($"Loaded Airline: {airlineCode} - {airlineName}");
                        }
                    }
                }

                Console.WriteLine("Airlines loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads boarding gates from a CSV file and adds them to a dictionary.
        /// This is Feature 1: Loading Boarding Gates
        /// </summary>
        /// 

        // rphl edit: added boarding_gate_dict as argument
        static void LoadBoardingGatesCSV(Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            try
            {
                using (StreamReader reader = new StreamReader("boardinggates.csv"))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        if (data.Length >= 4)
                        {
                            string gateName = data[0].Trim();
                            bool supportsCFFT = data[1].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                            bool supportsDDJB = data[2].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                            bool supportsLWTT = data[3].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);

                            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);

                            boarding_gate_dict[gateName] = gate; // rphl edit: added every boardinggate object to boarding gate dict
                            //Console.WriteLine($"Loaded Boarding Gate: {gateName} (CFFT: {supportsCFFT}, DDJB: {supportsDDJB}, LWTT: {supportsLWTT})");
                        }
                    }
                }

                Console.WriteLine("Boarding gates loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

        static void DisplayMenu(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict, Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            bool exit_command = false;
            while (!exit_command)
            {
                try
                {
                    // leave it as it is, copy-pasted from sample output
                    Console.WriteLine("=============================================" +
                                      "\r\nWelcome to Changi Airport Terminal 5\r\n" +
                                      "=============================================" +
                                      "\r\n1. List All Airlines\r\n2. List Boarding Gates\r\n0. Exit");

                    Console.WriteLine("Please select your option:");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 0:
                            exit_command = true;
                            Console.WriteLine("Goodbye");
                            break;

                        case 1:
                            ListAllAirlines(airline_dict);
                            break;

                        case 2:
                            ListAllBoardingGates(boarding_gate_dict);
                            break;

                        default:
                            Console.WriteLine("Please enter the values listed in the menu");
                            break;

                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter an appropriate number!");
                }

            }
        }

        static void ListAllAirlines(Dictionary<string, Airline> airline_dict)
        {
            Console.WriteLine($"{"Airline Code",-15}{"Airline Name",-30}");
            foreach (var airline in airline_dict.Values)
            {
                Console.WriteLine($"{airline.Code,-15}{airline.Name,-30}");
            }
        }

        static void ListAllBoardingGates(Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            Console.WriteLine($"{"Gate Name",-15}{"Supports CFFT",-15}{"Supports DDJB",-15}{"Supports LWTT",-15}");
            foreach (var gate in boarding_gate_dict.Values)
            {
                Console.WriteLine($"{gate.gateName,-15}{gate.SupportsCFFT,-15}{gate.SupportsDDJB,-15}{gate.SupportsLWTT,-15}");
            }
        }

    }

}

/*
 * 
 * TODO:
 * 
 * display menu (partially done, merge conflict error with terminal.cs)
 * 
 * feature 1: load csv file (airline and boardinggates) DONE
 * feature 2: load csv file (flights) SKIPPED
 * feature 3: display flights information SKIPPED
 * feature 4: display boarding gates information SKIPPED
 * feature 5: assign boarding gate to a flight (SKIP FOR NOW)
 * feature 6: create new flight SKIPPED
 * feature 7: display full flight details from an airline SKIPPED
 * feature 8: modify flight details SKIPPED
 * feature 9: display scheduled flights (sorted in order -> IComparable) with boarding gates assignments SKIPPED
 * 
 * Raphael: 2,3,5,6,9
 * Yeaw Min: 1,4,7,8
 * 
 */
