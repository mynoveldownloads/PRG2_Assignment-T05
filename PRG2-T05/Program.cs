using System;
using System.Collections.Generic;
using System.IO;

namespace PRG2_T05_Flight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            Dictionary<string, Flight> flight_list = new Dictionary<string, Flight>();
            LoadAirlinesCSV();
            LoadBoardingGatesCSV();
            LoadFlightsCSV();
        }

        /// <summary>
        /// Loads airlines from a CSV file and adds them to a dictionary.
        /// This is Feature 1: Loading Airlines
        /// </summary>
        static void LoadAirlinesCSV()
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
                            Console.WriteLine($"Loaded Airline: {airlineCode} - {airlineName}");
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
        static void LoadBoardingGatesCSV()
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
                            Console.WriteLine($"Loaded Boarding Gate: {gateName} (CFFT: {supportsCFFT}, DDJB: {supportsDDJB}, LWTT: {supportsLWTT})");
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
        static void LoadFlightsCSV()
        {
            string[] file = File.ReadAllLines("flights.csv");
            for (int i = 1; i < file.Length; i++)
            {
                Console.WriteLine(file[i]);
                string[] split_text = file[i].Split(",");

                string flight_no = split_text[0];
                string origin = split_text[1];
                string destination = split_text[2];
                DateTime expected_time = DateTime.Parse(split_text[3]);
                string SpecialRequestCode = split_text[4];

                Flight new_flight = new Flight();
                // flight has string flight_no, string origin, string destination, DateTime expected_time, string status)
            }
        }
        //Flight Number,Origin,Destination,Expected Departure/Arrival,Special Request Code
        //SQ 115,Tokyo(NRT),Singapore(SIN),11:45 AM,DDJB
    }
}



/*
 * 
 * TODO:
 * 
 * feature 1: load csv file (airline and boardinggates)
 * feature 2: load csv file (flights)
 * feature 3: display flights informaion
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