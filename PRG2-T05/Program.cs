using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG2_T05_Flight;
using PRG2_T05_CFFTFlight;
using PRG2_T05_DDJBFlight;
using PRG2_T05_LWTTFlight;
using PRG2_T05_NORMFlight;

namespace PRG2_T05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Flight> flight_dict = new Dictionary<string, Flight>();
            LoadFlightsCSV(flight_dict);
            ListAllFlights(flight_dict);
        }

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
                string special_request_code = split_text[4];
                string status = GetStatus(special_request_code);

                if (special_request_code == "LWTT")
                {
                    double request_fee = 500;
                    LWTTFlight new_flight = new LWTTFlight(flight_no, origin, destination, expected_time, status, request_fee);
                    flight_dict[flight_no] = new_flight;
                }
                else if (special_request_code == "DDJB")
                {
                    double request_fee = 500;
                    DDJBFlight new_flight = new DDJBFlight(flight_no, origin, destination, expected_time, status, request_fee);
                    flight_dict[flight_no] = new_flight;
                }
                else if (special_request_code == "CFFT")
                {
                    double request_fee = 500;
                    CFFTFlight new_flight = new CFFTFlight(flight_no, origin, destination, expected_time, status, request_fee);
                    flight_dict[flight_no] = new_flight;
                }
            }
            Console.WriteLine("flights.csv has been initialised\n");
        }

        static string GetStatus(string special_request_code)
        {
            if (special_request_code == "LWTT") return "Delay";
            else if (special_request_code == "DDJB") return "On Time";
            else if (special_request_code == "CFFT") return "Boarding";

            return "None";
        }

        static void ListAllFlights(Dictionary<string, Flight> flight_dict)
        {
            Console.WriteLine($"{"Flight Number", -16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-31}");
            foreach (var  flight in flight_dict.Values)
            {

                Console.WriteLine($"{flight.FlightNumber,-16}{GetAirlineName(flight.FlightNumber),-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-31}");
            }
        }

        static string GetAirlineName(string airline_code) // works
        {
            string code = airline_code.Substring(0, 2); // perplexity help, extract the first 2 letters (airline code) to obtain the airline name
            
            Dictionary<string, string> airline_name_dict = new Dictionary<string, string>();

            // initiate airlines.csv
            string[] airline_csv = File.ReadAllLines("airlines.csv");
            for (int i = 1; i < airline_csv.Length; i++)
            {
                string[] split_text = airline_csv[i].Split(",");
                string extracted_name = split_text[0];
                string extracted_code = split_text[1];

                airline_name_dict[extracted_code] = extracted_name;
            }
            string airline_name = airline_name_dict[code];

            return airline_name;
        }
    }
}


/*
 * 
 * TODO:
 * 
 * feature 1: load csv file (airline and boardinggates)
 * feature 2: load csv file (flights) DONE
 * feature 3: display flights informaion DONE
 * feature 4: display boarding gates information
 * feature 5: assign boarding gate to a flight
 * feature 6: create new flight
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