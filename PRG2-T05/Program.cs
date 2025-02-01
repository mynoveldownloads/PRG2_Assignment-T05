//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PRG2_T05_CFFTFlight;
using PRG2_T05_DDJBFlight;
using PRG2_T05_Flight;
using PRG2_T05_LWTTFlight;
using PRG2_T05_NORMFlight;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PRG2_T05_Flight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Flight> flight_dict = new Dictionary<string, Flight>();
            Dictionary<string, Airline> airline_dict = new Dictionary<string, Airline>();
            Dictionary<string, BoardingGate> boarding_gate_dict = new Dictionary<string, BoardingGate>();
            Dictionary<string, double> gate_fees = new Dictionary<string, double>();
            gate_fees["Changi Airport Terminal 5"] = 300;



            // Create Terminal
            Terminal terminal = new Terminal("Changi Airport Terminal 5", airline_dict, flight_dict, gate_fees, boarding_gate_dict);

            // Load CSV Data
            LoadAirlinesCSV(airline_dict);
            LoadBoardingGatesCSV(boarding_gate_dict);
            LoadFlightsCSV(flight_dict, airline_dict, terminal);

            // Pass Everything to Menu
            DisplayMenu(flight_dict, airline_dict, boarding_gate_dict, terminal);
        }



        /// <summary>
        /// Loads airlines from a CSV file and adds them to a dictionary.
        /// This is Feature 1: Loading Airlines
        /// </summary>
        /// 
        static void LoadAirlinesCSV(Dictionary<string, Airline> airline_dict)
        {
            try
            {
                Console.WriteLine("Loading Airlines...");
                var data = File.ReadAllLines("airlines.csv");

                for (int i = 1; i < data.Length; i++)
                {
                    string[] splitData = data[i].Split(",");
                    string airlineName = splitData[0];
                    string airlineCode = splitData[1];

                    Airline airline = new Airline(name: airlineName, code: airlineCode);
                    airline_dict[airlineCode] = airline;
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
        static void LoadBoardingGatesCSV(Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            try
            {
                Console.WriteLine("Loading Boarding Gates...");
                var data = File.ReadAllLines("boardinggates.csv");

                for (int i = 1; i < data.Length; i++)
                {
                    string[] dataSplit = data[i].Split(",");
                    string gateName = dataSplit[0];

                    bool supportsDDJB = bool.Parse(dataSplit[1]);
                    bool supportsCFFT = bool.Parse(dataSplit[2]);
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

        static List<string>? CheckFlightAssignment(string flight_no, Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            foreach (var gate_name in boarding_gate_dict.Keys)
            {
                if (boarding_gate_dict[gate_name].AssignedFlight != null &&
                    boarding_gate_dict[gate_name].AssignedFlight.FlightNumber == flight_no)
                {
                    return new List<string> { flight_no, gate_name };
                }
            }
            return null;
        }

        static bool? AcceptRequestCode(string special_request_code, BoardingGate boarding_gate)
        {
            List<string>? strings = null;
            var check_request_code = new Dictionary<string, bool>
            {
                { "DDJB", boarding_gate.SupportsDDJB },
                { "CFFT", boarding_gate.SupportsCFFT },
                { "LWTT", boarding_gate.SupportsLWTT }
            };

            if (check_request_code.ContainsKey(special_request_code))
            {
                return check_request_code[special_request_code];
            }
            else if (special_request_code == "NORM")
            {
                return true; // NORMFlight can use any unassigned boarding gate without restrictions
            }
            else
            {
                return null; // return null if the input special request code is foreign
            }

        }

        static void AssignBoardingGateToFlight(Dictionary<string, BoardingGate> boarding_gate_dict, Dictionary<string, Flight> flight_dict)
        {
            bool exit_method = false;
            while (!exit_method)
            {
                try
                {
                    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\nEnter Flight Number:");
                    string flight_no = Console.ReadLine().ToUpper();

                    if (flight_no == "--") { exit_method = true; Console.WriteLine("Method exited."); break; } // trigger command to exit method

                    if (!flight_dict.ContainsKey(flight_no))
                    {
                        Console.WriteLine($"Flight number \"{flight_no}\" does not exist in Flight dictionary.");
                        continue;
                    }
                    else if (CheckFlightAssignment(flight_no, boarding_gate_dict) != null)
                    {
                        List<string> assigned_flights = CheckFlightAssignment(flight_no, boarding_gate_dict);
                        Console.WriteLine($"An error occurred! Flight number \"{assigned_flights[0]}\" already exists in gate \"{assigned_flights[1]}\"");
                        continue;
                    }
                    string special_request_code = flight_dict[flight_no].GetType().Name.Substring(0, 4);

                    Console.WriteLine("Enter Boarding Gate Name:");
                    string gate_name = Console.ReadLine().ToUpper();

                    if (!boarding_gate_dict.ContainsKey(gate_name))
                    {
                        Console.WriteLine($"Gate name \"{gate_name}\" does not exist in Boarding Gates dictionary.");
                        continue;
                    }
                    else
                    {
                        bool? result = AcceptRequestCode(special_request_code, boarding_gate_dict[gate_name]);
                        if (result == false)
                        {
                            Console.WriteLine($"Boarding Gate \"{gate_name}\" does not accept Flight \"{flight_no}\" as it does not support \"{special_request_code}\""); continue;
                        }
                        else if (result == null)
                        {
                            Console.WriteLine($"Special request code \"{special_request_code}\" is not recognised."); continue;
                        }
                    }


                    BoardingGate boarding_gate = boarding_gate_dict[gate_name];

                    if (boarding_gate.AssignedFlight != null)
                    {
                        Console.WriteLine($"An error occurred! Boarding gate \"{gate_name}\" is already assigned to flight {boarding_gate.AssignedFlight.FlightNumber}.");
                        continue;
                    }

                    Flight selected_flight = flight_dict[flight_no];

                    Console.WriteLine($"Supports DDJB: {boarding_gate.SupportsDDJB}");
                    Console.WriteLine($"Supports CFFT: {boarding_gate.SupportsCFFT}");
                    Console.WriteLine($"Supports LWTT: {boarding_gate.SupportsLWTT}");

                    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                    string choice = Console.ReadLine().ToUpper();

                    string[] yes_or_no = { "Y", "N" };
                    if (!yes_or_no.Contains(choice))
                    {
                        Console.WriteLine("Please enter Y or N only!");
                        continue;
                    }

                    Dictionary<int, string> choices = new Dictionary<int, string>
                    {
                        { 1, "Delayed" },
                        { 2, "Boarding" },
                        { 3, "On Time" }
                    };

                    string status = "";
                    switch (choice)
                    {
                        case "Y":
                            Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time\r\nPlease select the new status of the flight:");
                            try
                            {
                                int choice2 = int.Parse(Console.ReadLine());

                                if (choices.ContainsKey(choice2))
                                {
                                    status = choices[choice2];
                                    selected_flight.Status = status;
                                    boarding_gate.AssignedFlight = selected_flight;
                                    Console.WriteLine($"Flight {flight_no} has been assigned to Boarding Gate {gate_name}!");
                                    exit_method = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Please enter values 1 to 3 only!"); continue;
                                }
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine("Enter values 1 to 3 only!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"An error occurred! {e}");
                            }
                            break;
                        case "N":
                            status = choices[3];
                            selected_flight.Status = status;
                            boarding_gate.AssignedFlight = selected_flight;
                            Console.WriteLine($"Flight {flight_no} has been assigned to Boarding Gate {gate_name}!");
                            exit_method = true;
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter a valid flight number");
                }
            }
        }

        static void LoadFlightsCSV(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict, Terminal terminal)
        {
            Console.WriteLine("Loading Flights...");
            string[] file = File.ReadAllLines("flights.csv");

            for (int i = 1; i < file.Length; i++)
            {
                string[] split_text = file[i].Split(",");
                string flight_no = split_text[0];
                string origin = split_text[1];
                string destination = split_text[2];
                DateTime expected_time = DateTime.Parse(split_text[3]);
                string special_request_code = split_text[4];

                // Extract airline code from flight number
                string airline_code = flight_no.Substring(0, 2);

                // Ensure airline exists before assigning flight
                if (!airline_dict.ContainsKey(airline_code))
                {
                    Console.WriteLine($"Warning: Airline {airline_code} not found for flight {flight_no}. Skipping...");
                    continue;
                }

                Flight flight;
                if (special_request_code == "LWTT")
                {
                    flight = new LWTTFlight(flight_no, origin, destination, expected_time, "Delayed");
                }
                else if (special_request_code == "DDJB")
                {
                    flight = new DDJBFlight(flight_no, origin, destination, expected_time, "On Time");
                }
                else if (special_request_code == "CFFT")
                {
                    flight = new CFFTFlight(flight_no, origin, destination, expected_time, "Boarding");
                }
                else
                {
                    flight = new NORMFlight(flight_no, origin, destination, expected_time, "Scheduled");
                }

                // Add flight to the flight dictionary
                flight_dict[flight_no] = flight;

                // Add flight to its respective airline using AddFlight
                airline_dict[airline_code].AddFlight(flight);

                // Also add flight to Terminal (optional, useful for centralization)
                terminal.Flights[flight_no] = flight;
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
            Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-31}");
            foreach (var flight in flight_dict.Values)
            {
                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}");
            }
        }

        static string GetAirlineName(Dictionary<string, Airline> airline_dict, string flightNumber)
        {
            string code = flightNumber.Substring(0, 2).Trim(); // Correct extraction
            return airline_dict[code].Name;
        }

        static bool IsNegative(double user_input)
        {
            return user_input < 0;
        }

        static void CreateNewFlight(Dictionary<string, Flight> flight_dict)
        {
            bool exit_method = false;
            while (!exit_method)
            {
                try
                {
                    Console.Write("Enter Flight Number: ");
                    string flight_no = Console.ReadLine().ToUpper();

                    if (flight_no == "--") { exit_method = true; Console.WriteLine("Method exited."); break; } // trigger command to exit method

                    if (flight_dict.ContainsKey(flight_no))
                    {
                        Console.WriteLine($"Flight \"{flight_no}\" already exists in flight list! enter a new flight."); continue;
                    }

                    Console.WriteLine("Enter Origin: ");
                    string origin = Console.ReadLine();
                    Console.WriteLine("Enter Destination: ");
                    string destination = Console.ReadLine();
                    Console.WriteLine("Enter Expected Departure/Arrival Time(dd/mm/yyyy hh:mm): ");
                    DateTime expected_time = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine("Enter Special Request Code(CFFT/DDJB/LWTT/None): ");
                    string special_request_code = Console.ReadLine().ToUpper();

                    string? status = null;
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
                    else if (special_request_code == "NONE")
                    {
                        NORMFlight new_flight = new NORMFlight(flight_no, origin, destination, expected_time, status);
                        flight_dict[flight_no] = new_flight;
                    }
                    else
                    {
                        Console.WriteLine("Please enter the given request codes only!"); continue;
                    }

                    try
                    {
                        using (StreamWriter writer = new StreamWriter("flights.csv", true))
                        {
                            writer.WriteLine($"{flight_no},{origin},{destination},{expected_time},{special_request_code}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    Console.WriteLine($"Flight {flight_no} has been added!");
                    Console.WriteLine("Would you like to add another flight? (Y/N)");
                    string choice = Console.ReadLine();
                    if (choice == "N") exit_method = true; break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter an appropriate value");
                }
            }
        }

        static void DisplayFlightSchedule(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict, Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            List<Flight> flights = flight_dict.Values.ToList();
            flights.Sort();
            Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");
            foreach (Flight flight in flights)
            {
                string assigned_boarding_gate = "Unassigned";
                var result = CheckFlightAssignment(flight.FlightNumber, boarding_gate_dict);
                if (result != null && result.Count >= 2)
                {
                    assigned_boarding_gate = result[1];
                }
                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-36}{"Scheduled",-16}{assigned_boarding_gate,-13}");
            }
        }

        // FEATURE 4: List Boarding Gates (Original Implementation)
        static void ListBoardingGates(Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
            Console.WriteLine("Gate Name       DDJB                   CFFT                   LWTT");
            foreach (var gate in boarding_gate_dict.Values
    .OrderBy(g => new string(g.gateName.TakeWhile(char.IsLetter).ToArray())) // Sort by the prefix (e.g., "A", "B", "C")
    .ThenBy(g => int.Parse(new string(g.gateName.SkipWhile(c => !char.IsDigit(c)).ToArray())))) // Sort by the numeric part
            {
                Console.WriteLine($"{gate.gateName,-16}{gate.SupportsDDJB,-23}{gate.SupportsCFFT,-23}{gate.SupportsLWTT,-23}");
            }


        }

        // FEATURE 7: Display Airline Flights (Original Implementation)
        static void DisplayAirlineFlights(Dictionary<string, Airline> airline_dict, Dictionary<string, Flight> flight_dict, Dictionary<string, BoardingGate> boarding_gate_dict)
        {
            Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
            Console.WriteLine("Airline Code\tAirline Name");
            foreach (var airline in airline_dict.Values)
            {
                Console.WriteLine($"{airline.Code}\t\t{airline.Name}");
            }

            Console.Write("Enter Airline Code: ");
            string code = Console.ReadLine().ToUpper();

            if (airline_dict.TryGetValue(code, out Airline selectedAirline))
            {
                Console.WriteLine($"=============================================\r\nList of Flights for {selectedAirline.Name}\r\n=============================================");
                Console.WriteLine("Flight Number\tOrigin\t\tDestination\tExpected Time");
                foreach (var flight in flight_dict.Values.Where(f => f.FlightNumber.StartsWith(code)))
                {
                    var gate = boarding_gate_dict.Values.FirstOrDefault(g => g.AssignedFlight?.FlightNumber == flight.FlightNumber);
                    Console.WriteLine($"{flight.FlightNumber}\t{flight.Origin}\t{flight.Destination}\t{flight.ExpectedTime}\t{(gate != null ? gate.gateName : "Unassigned")}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Airline Code!");
            }
        }

        // FEATURE 8: Modify Flight Details (Original Implementation)
        static void ModifyFlightDetails(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
                    foreach (var airline in airline_dict.Values)
                    {
                        Console.WriteLine($"{airline.Code} - {airline.Name}");
                    }

                    Console.Write("Enter Airline Code: ");
                    string code = Console.ReadLine().ToUpper();

                    if (code == "--") { Console.WriteLine("Method exited."); break; } // trigger command to exit method

                    if (!airline_dict.ContainsKey(code))
                    {
                        Console.WriteLine("Invalid Airline Code!");
                        continue;

                    }

                    var airlineFlights = flight_dict.Values
                        .Where(f => f.FlightNumber.StartsWith(code))
                        .ToList();

                    Console.WriteLine("=============================================");
                    foreach (var flight in airlineFlights)
                    {
                        Console.WriteLine($"{flight.FlightNumber} - {flight.Origin} to {flight.Destination}");
                    }

                    Console.Write("Enter Flight Number: ");
                    string flightNumber = Console.ReadLine().ToUpper();

                    if (!flight_dict.ContainsKey(flightNumber))
                    {
                        Console.WriteLine("Flight not found!");
                        continue;
                    }

                    Console.WriteLine("1. Modify Flight\n2. Delete Flight");
                    Console.Write("Choose option: ");
                    int option = int.Parse(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            Console.Write("New Origin: ");
                            string newOrigin = Console.ReadLine();
                            Console.Write("New Destination: ");
                            string newDest = Console.ReadLine();
                            Console.Write("New Expected Time (dd/MM/yyyy HH:mm): ");
                            DateTime newTime = DateTime.Parse(Console.ReadLine());

                            flight_dict[flightNumber].Origin = newOrigin;
                            flight_dict[flightNumber].Destination = newDest;
                            flight_dict[flightNumber].ExpectedTime = newTime;
                            Console.WriteLine("Flight updated successfully!");
                            break;

                        case 2:
                            flight_dict.Remove(flightNumber);
                            Console.WriteLine("Flight deleted successfully!");
                            break;

                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Please enter an appropriate input!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred! {e.Message}");
                }
            }
        }

        static List<string> ListOutFlightSupport(BoardingGate boarding_gate)
        {
            List<string> list_of_flight_support = new List<string>
            {
                boarding_gate.SupportsDDJB ? "DDJB" : "NORM",
                boarding_gate.SupportsCFFT ? "CFFT" : "NORM",
                boarding_gate.SupportsLWTT ? "LWTT" : "NORM"
            };
            return list_of_flight_support;
        }

        static void ProcessUnassignedFlights(Dictionary<string, BoardingGate> boarding_gate_dict, Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict)
        {
            Console.WriteLine("=============================================\r\nProcessing Unassigned Flights for Changi Airport Terminal 5\r\n=============================================");

            // check if each boardinggate has an assigned flight, if it has null assignedflight, add it to a queue
            Dictionary<string, List<string>> vacant_boarding_gates = new Dictionary<string, List<string>>();

            Queue<Flight> unassigned_flight = new Queue<Flight>();
            double manually_assigned_flights = 0;

            foreach (var flight in flight_dict.Values)
            {
                // returns flight_no, gate_name
                List<string>? assigned_flight = CheckFlightAssignment(flight.FlightNumber, boarding_gate_dict);
                if (assigned_flight == null)
                {
                    unassigned_flight.Enqueue(flight);
                }
                else
                {
                    manually_assigned_flights++;
                }
            }
            Console.WriteLine($"There are {unassigned_flight.Count} unassigned flights");

            int num_unassigned_boarding_gates = 0;

            foreach (var boarding_gate in boarding_gate_dict.Values)
            {
                if (boarding_gate.AssignedFlight == null) // check if boarding gate is vacant
                {
                    num_unassigned_boarding_gates++;
                    vacant_boarding_gates[boarding_gate.gateName] = ListOutFlightSupport(boarding_gate); // if the list contains all 3 values of "NORM", reserve it for NORMFLight
                }
            }

            Console.WriteLine($"There are {num_unassigned_boarding_gates} unassigned boarding gates\n");

            double automatically_assigned_flights = 0;

            if (automatically_assigned_flights != 0)
            {

                Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/ Arrival Time",-36}{"Special Request Code",-16}{"Assigned Boarding Gate",-23}");
            }
            while (unassigned_flight.Count > 0)
            {
                var flight = unassigned_flight.Dequeue();
                string special_request_code = flight.GetType().Name.Substring(0, 4);

                foreach (string gate_name in vacant_boarding_gates.Keys)
                {
                    // assign flights that are not NORM
                    // if flight is not NORM and all 3 values in the list of the supported flights in the gate is not "NORM", meaning the list contains at least DDJB, CFFT or LWTT
                    if (special_request_code != "NORM" && vacant_boarding_gates[gate_name].Contains(special_request_code))
                    {

                        // if a flight is supported in a flight, flight is assigned to the gate name in boarding_gate_dict
                        boarding_gate_dict[gate_name].AssignedFlight = flight;
                        vacant_boarding_gates.Remove(gate_name); // once a flight is already assigned to a gate, remove the gate from vacant_boarding_gates
                        automatically_assigned_flights++;
                        Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-36}{special_request_code,-16}{gate_name,-23}");
                        break;

                    }
                    else if (special_request_code == "NORM" && vacant_boarding_gates[gate_name].All(item => item == "NORM")) // if gate support list contains all NORM, assign NORM flights to this gate
                    {
                        boarding_gate_dict[gate_name].AssignedFlight = flight;
                        vacant_boarding_gates.Remove(gate_name);
                        automatically_assigned_flights++;
                        Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(airline_dict, flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-36}{special_request_code,-16}{gate_name,-23}");
                        break;
                    }
                }
            }
            double total_assigned_flights = manually_assigned_flights + automatically_assigned_flights;
            double percentage_of_processed_flights = (automatically_assigned_flights / total_assigned_flights) * 100;
            Console.WriteLine($"There is a total of {automatically_assigned_flights} processed flights and boarding gates.\n{percentage_of_processed_flights:F2}% were automatically generated.");
        }

        static void DisplayMenu(Dictionary<string, Flight> flight_dict, Dictionary<string, Airline> airline_dict, Dictionary<string, BoardingGate> boarding_gate_dict, Terminal terminal)
        {
            bool exit_command = false;
            while (!exit_command)
            {
                try
                {
                    Console.WriteLine("\n=============================================");
                    Console.WriteLine("Welcome to Changi Airport Terminal 5");
                    Console.WriteLine("=============================================");
                    Console.WriteLine("1. List All Flights");
                    Console.WriteLine("2. List Boarding Gates");
                    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
                    Console.WriteLine("4. Create Flight");
                    Console.WriteLine("5. Display Airline Flights");
                    Console.WriteLine("6. Modify Flight Details");
                    Console.WriteLine("7. Display Flight Schedule");
                    Console.WriteLine("8. Process Unassigned Flights");
                    Console.WriteLine("9. Display Airline Fees");
                    Console.WriteLine("0. Exit");
                    Console.Write("Please select your option: ");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 0:
                            exit_command = true;
                            Console.WriteLine("Goodbye!");
                            break;
                        case 1:
                            ListAllFlights(flight_dict, airline_dict);
                            break;
                        case 2:
                            ListBoardingGates(boarding_gate_dict);
                            break;
                        case 3:
                            AssignBoardingGateToFlight(boarding_gate_dict, flight_dict);
                            break;
                        case 4:
                            CreateNewFlight(flight_dict);
                            break;
                        case 5:
                            DisplayAirlineFlights(airline_dict, flight_dict, boarding_gate_dict);
                            break;
                        case 6:
                            ModifyFlightDetails(flight_dict, airline_dict);
                            break;
                        case 7:
                            DisplayFlightSchedule(flight_dict, airline_dict, boarding_gate_dict);
                            break;
                        case 8:
                            ProcessUnassignedFlights(boarding_gate_dict, flight_dict, airline_dict);
                            break;
                        case 9:
                            terminal.PrintAirlineFees();
                            break;
                        default:
                            Console.WriteLine("Please enter a valid option.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }
    }
}