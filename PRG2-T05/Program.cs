//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


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
                Console.WriteLine("Loading Airlines...");
                using (StreamReader reader = new StreamReader("airlines.csv"))
                {
                    //string? line;
                    //while ((line = reader.ReadLine()) != null) // -> old code, takes in header row as part of dataset
                    //{
                    //    Console.WriteLine($"{line}");
                    //    var data = line.Split(',');
                    //    if (data.Length >= 2)
                    //    {
                    //        string airlineName = data[0].Trim();                            
                    //        string airlineCode = data[1].Trim();
                    //        Airline airline = new Airline(name: airlineName, code: airlineCode);
                    //        airline_dict[airlineCode] = airline; // rphl edit: added every airline object to airline dict
                    //                                             // airline_dict in main method is updated
                    //        //Console.WriteLine($"Loaded Airline: {airlineCode} - {airlineName}");
                    //    }
                    //}

                    var data = File.ReadAllLines("airlines.csv");
                    for (int i = 1; i < data.Length; i++) // recommended fix (rphl)
                    {
                        //Console.WriteLine($"{data[i]}");
                        string[] splitData = data[i].Split(",");
                        string airlineName = splitData[0];
                        string airlineCode = splitData[1];
                        Airline airline = new Airline(name: airlineName, code: airlineCode);
                        airline_dict[airlineCode] = airline;

                    }
                }
                Console.WriteLine($"{airline_dict.Count} Airlines Loaded!");
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
                Console.WriteLine("Loading Boarding Gates...");
                //using (StreamReader reader = new StreamReader("boardinggates.csv")) // -> old code, same issue as load airlines csv method
                //{
                //    string? line;
                //    while ((line = reader.ReadLine()) != null)
                //    {
                //        var data = line.Split(',');
                //        if (data.Length >= 4)
                //        {
                //            string gateName = data[0].Trim();
                //            bool supportsCFFT = data[1].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                //            bool supportsDDJB = data[2].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                //            bool supportsLWTT = data[3].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);

                //            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);

                //            boarding_gate_dict[gateName] = gate; // rphl edit: added every boardinggate object to boarding gate dict
                //            //Console.WriteLine($"Loaded Boarding Gate: {gateName} (CFFT: {supportsCFFT}, DDJB: {supportsDDJB}, LWTT: {supportsLWTT})");
                //            // boarding gate dict with gateName as key
                //        }
                //    }
                //}

                var data = File.ReadAllLines("boardinggates.csv"); // -> updated code (rphl)
                for (int i = 1; i < data.Length; i++)
                {
                    string[] dataSplit = data[i].Split(",");
                    string gateName = dataSplit[0];
                    bool supportsCFFT = bool.Parse(dataSplit[1]);
                    bool supportsDDJB = bool.Parse(dataSplit[2]);
                    bool supportsLWTT = bool.Parse(dataSplit[3]);

                    BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);

                    boarding_gate_dict[gateName] = gate;
                }

                Console.WriteLine($"{boarding_gate_dict.Count} Boarding Gates Loaded!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

        static List<string>? CheckFlightAssignment(string flight_no, Dictionary<string, BoardingGate> boarding_gate_dict) // check if flight is already assigned to a boarding gate
        {
            //List<string>? found_assigned_flight = null;
            foreach (var gate_name in  boarding_gate_dict.Keys)
            {
                if (boarding_gate_dict[gate_name].AssignedFlight != null && boarding_gate_dict[gate_name].AssignedFlight.FlightNumber == flight_no)
                {
                    //found_assigned_flight = new List<string> { flight_no, gate_name}; break;
                    return new List<string> { flight_no, gate_name };
                }
            }

            //return found_assigned_flight;
            return null;
        }

        // option 3
        static void AssignBoardingGateToFlight (Dictionary<string, BoardingGate> boarding_gate_dict, Dictionary<string, Flight> flight_dict)
        {
            bool exit_method = false;
            while (!exit_method)
            {
                try
                {
                    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\nEnter Flight Number:");
                    string flight_no = Console.ReadLine().ToUpper();

                    if (!flight_dict.ContainsKey(flight_no))
                    {
                        Console.WriteLine($"Flight number \"{flight_no}\" does not exist in Flight dictionary."); continue;
                    }

                    else if (CheckFlightAssignment(flight_no, boarding_gate_dict) != null) // check if a flight is already assigned to a boarding gate
                    {
                        List<string> assigned_flights = CheckFlightAssignment(flight_no, boarding_gate_dict);
                        string found_flight_no = assigned_flights[0];
                        string found_gate_name = assigned_flights[1];
                        Console.WriteLine($"An error occurred! Flight number {found_flight_no} already exists in gate {found_gate_name}"); continue;
                    }

                    Console.WriteLine("Enter Boarding Gate Name:\n");
                    string gate_name = Console.ReadLine().ToUpper();

                    if (!boarding_gate_dict.ContainsKey(gate_name))
                    {
                        Console.WriteLine($"Gate name \"{gate_name}\" does not exist in Boarding Gates dictionary."); continue ;
                    }

                    BoardingGate boarding_gate = boarding_gate_dict[gate_name];

                    if (boarding_gate.AssignedFlight != null)
                    {
                        Console.WriteLine($"An error occurred! Boarding gate {gate_name} is already assigned to flight {boarding_gate.AssignedFlight.FlightNumber}."); continue;
                    }

                    Console.WriteLine($"Supports DDJB: {boarding_gate.SupportsDDJB}");
                    Console.WriteLine($"Supports CFFT: {boarding_gate.SupportsCFFT}");
                    Console.WriteLine($"Supports LWTT: {boarding_gate.SupportsLWTT}");

                    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                    string choice = Console.ReadLine().ToUpper();

                    string[] yes_or_no = { "Y", "N" };
                    if (!yes_or_no.Contains(choice))
                    {
                        Console.WriteLine("Please enter Y or N only!"); continue;
                    }

                    Dictionary<int, bool> dict_of_support = new Dictionary<int, bool>();
                    dict_of_support[1] = boarding_gate.SupportsLWTT;
                    dict_of_support[2] = boarding_gate.SupportsCFFT;
                    dict_of_support[3] = boarding_gate.SupportsDDJB;

                    //var eg_object = flight_dict["SQ 115"]; // example debug code
                    //boarding_gate.AssignedFlight = eg_object; // check if duplication is possible or not
                    //boarding_gate_dict["A20"] = boarding_gate;
                    switch (choice)
                    /*
                        * // LWTT -> Delay
                        // DDJB -> On Time
                        // CFFT -> Boarding
                        */
                    {
                        case "Y":
                            Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time\r\nPlease select the new status of the flight:");

                            // 1 -> LWTT, 2 -> CFFT, 3 -> DDJB
                            try
                            {
                                int choice2 = int.Parse(Console.ReadLine());
                                // if true, add flight to boardinggate
                                if (dict_of_support.ContainsKey(choice2))
                                {
                                    if (dict_of_support[choice2] == true)
                                    {

                                        if (boarding_gate.AssignedFlight == null)
                                        {

                                            boarding_gate.AssignedFlight = flight_dict[flight_no];
                                            boarding_gate_dict[gate_name] = boarding_gate;
                                            Console.WriteLine(boarding_gate_dict[gate_name].ToString());
                                            Console.WriteLine($"Flight {flight_no} has been assigned to Boarding Gate {gate_name}!");
                                            exit_method = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"An error occured! Flight {boarding_gate.AssignedFlight.FlightNumber} is already assigned to gate {gate_name}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Select an option that is True");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Enter number 1 to 3 only");
                                }
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine("Enter values 1 to 3 only!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"AN error occurred: {e}");
                            }

                            break;
                        case "N":
                            Console.WriteLine("Method exited.");
                            exit_method = true;
                            break;

                        default:
                            Console.WriteLine("Please enter Y or N only");
                            break;
                    }
                    
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter a valid flight number");
                }
            }
        }


        /// <summary>
        /// Loads flights from a CSV file and adds them to a dictionary.
        /// This is Feature 2: Loading Flights
        /// </summary>


        // edited to fix feature 5
        static void LoadFlightsCSV (Dictionary<string, Flight> flight_dict)
        {
            Console.WriteLine("Loading Flights...");
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
                else
                {
                    NORMFlight new_flight = new NORMFlight(flight_no, origin, destination, expected_time, status); // added this for feature 5
                    flight_dict[flight_no] = new_flight; // feature 5 fixed, listallflights method fixed to display all flights including null special codes
                }
            }
            Console.WriteLine($"{flight_dict.Count} Flights Loaded!");
        }

        static string? GetStatus(string special_request_code)
        {
            if (special_request_code == "LWTT") return "Delay";
            else if (special_request_code == "DDJB") return "On Time";
            else if (special_request_code == "CFFT") return "Boarding";

            return null;
        }

        static void ListAllFlights(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict)
        {
            Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================");
            Console.WriteLine($"{"Flight Number", -16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-31}");
            foreach (var  flight in flight_dict.Values)
            {
                //Console.WriteLine(flight.GetType());
                //Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}"); // old code, leave it here
                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}");
            }
        }

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

        static void DIsplayFlightSchedule(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict, Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            List<Flight> flights = flight_dict.Values.ToList();
            flights.Sort();
            Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");
            //Console.WriteLine($"{flight.FlightNumber:-16}{GetAirlineName(airline_dict, flight.FlightNumber):-23}{flight.Origin:-23}{flight.Destination:-23}{flight.ExpectedTime:-36}{"Scheduled":-16}{assigned_boarding_gate:-13}");
            foreach (Flight flight in flights)
            {
                //Console.WriteLine($" {flight.FlightNumber}, {GetAirlineName(airline_dict, flight.FlightNumber)} {flight.ExpectedTime}");
                //string assigned_boarding_gate = CheckFlightAssignment(flight.FlightNumber, boarding_gate_dict)[1] ?? "Unassigned";
                string assigned_boarding_gate = "Unassigned";
                var result = CheckFlightAssignment(flight.FlightNumber, boarding_gate_dict);
                if (result != null)
                {
                     if (result.Count >= 2) { assigned_boarding_gate = result[1]; }
                }

                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-36}{"Scheduled",-16}{assigned_boarding_gate,-13}");
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
                    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");

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

                        case 2: // feature 2 - yeaw min
                            // display boarding gates information
                            break;

                        case 3:
                            AssignBoardingGateToFlight(boarding_gate_dict, flight_dict);
                            break;

                        case 4: // feature 6 - rphl
                            // create new flight
                            CreateNewFlight(flight_dict);
                            break;

                        case 5: // feature 7 - yeaw min
                            // display airline flights
                            break;

                        case 6: // modify flight details - yeaw min
                            // feature 8
                            break;

                        case 7: // feature 9 - rphl
                            // display flight schedule
                            DIsplayFlightSchedule(flight_dict, airline_dict, boarding_gate_dict);
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
    }
}



/*
 * 
 * TODO:
 * 
 * display menu (partially done, merge conflict error with terminal.cs)
 * 
 * feature 1: load csv file (airline and boardinggates)
 * feature 2: load csv file (flights) DONE
 * feature 3: display flights informaion DONE
 * feature 4: display boarding gates information
 * feature 5: assign boarding gate to a flight DONE
 * feature 6: create new flight DONE
 * feature 7: display full flight details from an airline
 * feature 8: modify flight details
 * feature 9: display scheduled flights (sorted in order -> IComparable) with boarding gates assignments
 * 
 * Raphael: 2,3,5,6,9
 * Yeaw Min: 1,4,7,8
 * 
 */

// LWTT -> Delay
// DDJB -> On Time
// CFFT -> Boarding
