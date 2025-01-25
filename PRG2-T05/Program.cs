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
            LoadFlightsCSV(flight_dict);
            DisplayMenu(flight_dict, airline_dict, boarding_gate_dict);

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

                            string airlineName = data[0].Trim();                            
                            string airlineCode = data[1].Trim();
                            Airline airline = new Airline(name: airlineName, code: airlineCode);
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


        /// <summary>
        /// Loads flights from a CSV file and adds them to a dictionary.
        /// This is Feature 2: Loading Flights
        /// </summary>

        static void LoadFlightsCSV (Dictionary<string, Flight> flight_dict)

        {
            string[] file = File.ReadAllLines("flights.csv");
            for (int i = 1; i < file.Length; i++)
            {
                string[] split_text = file[i].Split(",");

                // parsing each row into individual attributes to be stored in dictionary
                string flight_no = split_text[0];
                string origin = split_text[1];
                string destination = split_text[2];
                DateTime expected_time = DateTime.Parse(split_text[3]);

                string SpecialRequestCode = split_text[4];

                string special_request_code = split_text[4];
                string status = GetStatus(special_request_code);


                if (special_request_code == "LWTT")
                {
                    // old code, leaving it here if needed in future
                    //double request_fee = 500;
                    //LWTTFlight new_flight = new LWTTFlight(flight_no, origin, destination, expected_time, status, request_fee);
                    //flight_dict[flight_no] = new_flight;

                    LWTTFlight new_flight = new LWTTFlight(flight_no, origin, destination, expected_time, status);
                    flight_dict[flight_no] = new_flight;
                }
                else if (special_request_code == "DDJB")
                {
                    DDJBFlight new_flight = new DDJBFlight(flight_no, origin, destination, expected_time, status);
                    flight_dict[flight_no] = new_flight;
                }
                else if (special_request_code == "CFFT")
                {
                    CFFTFlight new_flight = new CFFTFlight(flight_no, origin, destination, expected_time, status);
                    flight_dict[flight_no] = new_flight;
                }
            }
            Console.WriteLine("Flights loaded successfully.\n");
        }

        static string GetStatus(string special_request_code)
        {
            if (special_request_code == "LWTT") return "Delay";
            else if (special_request_code == "DDJB") return "On Time";
            else if (special_request_code == "CFFT") return "Boarding";

            return "None";
        }

        static void ListAllFlights(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict)
        {
            Console.WriteLine($"{"Flight Number", -16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-31}");
            foreach (var  flight in flight_dict.Values)
            {

                //Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}"); // old code, leave it here
                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}");
            }
        }

        //static string GetAirlineName(string airline_code) // old code, leaving it here in case needed in future
        //{
        //    string code = airline_code.Substring(0, 2); // extract the first 2 letters (airline code) to obtain the airline name
            
        //    Dictionary<string, string> airline_name_dict = new Dictionary<string, string>();

        //    // initiate airlines.csv
        //    string[] airline_csv = File.ReadAllLines("airlines.csv");
        //    for (int i = 1; i < airline_csv.Length; i++)
        //    {
        //        string[] split_text = airline_csv[i].Split(",");
        //        string extracted_name = split_text[0];
        //        string extracted_code = split_text[1];

        //        airline_name_dict[extracted_code] = extracted_name;
        //    }
        //    string airline_name = airline_name_dict[code];

        //    return airline_name;
        //}

        static string GetAirlineName(Dictionary<string, Airline> airline_dict, string airline_code)
        {
            string code = airline_code.Substring(0, 2);
            //Console.WriteLine(code);
            //return "ha";
            return airline_dict[code].Name;
        }

        static bool IsNegative(double user_input)
        {
            if (user_input < 0) return true; else return false;
        }

        static void CreateNewFlight(Dictionary<string, Flight> flight_dict) // option 4
        {
            bool exit_method = false;

            while (!exit_method)
            {
                try
                {
                    Console.Write("Enter Flight Number: "); string flight_no = Console.ReadLine();
                    Console.WriteLine("Enter Origin: "); string origin = Console.ReadLine();
                    Console.WriteLine("Enter Destination: "); string destination = Console.ReadLine();
                    Console.WriteLine("Enter Expected Departure/Arrival Time(dd/mm/yyyy hh:mm): "); DateTime expected_time = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine("Enter Special Request Code(CFFT/DDJB/LWTT/None): "); string special_request_code = Console.ReadLine();

                    // ***NOTE: Flight class is abstract, cant be instantiated directly
                    string status = GetStatus(special_request_code);
                    if (special_request_code == "LWTT")
                    {
                        LWTTFlight new_flight = new LWTTFlight(flight_no, origin, destination, expected_time, status);
                        flight_dict[flight_no] = new_flight;
                    }
                    else if (special_request_code == "DDJB")
                    {
                        DDJBFlight new_flight = new DDJBFlight(flight_no, origin, destination, expected_time, status);
                        flight_dict[flight_no] = new_flight;
                    }
                    else if (special_request_code == "CFFT")
                    {
                        CFFTFlight new_flight = new CFFTFlight(flight_no, origin, destination, expected_time, status);
                        flight_dict[flight_no] = new_flight;
                    }

                    // append new flight to flights.csv file
                    try
                    {
                        using (StreamWriter writer = new StreamWriter("flights.csv", true))
                        {
                            writer.WriteLine($"{flight_no},{origin},{destination},{expected_time},{special_request_code}");
                        }
                        //Console.WriteLine($"Flight {flight_no} has been added to flight.csv");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }


                    Console.WriteLine($"Flight {flight_no} has been added!");

                    Console.WriteLine("Would you like to add another flight? (Y/N)"); string choice = Console.ReadLine();

                    if (choice == "Y") exit_method = true; else if (choice == "N") continue; else Console.WriteLine("Invalid option!");

                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter an appropriate value");
                }
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

                            ListAllFlights(flight_dict, airline_dict);

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
