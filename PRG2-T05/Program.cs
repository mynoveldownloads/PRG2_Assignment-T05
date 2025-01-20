using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG2_T05_Flight;

namespace PRG2_T05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            Dictionary<string, Flight> flight_list = new Dictionary<string, Flight>();
            LoadFlightsCSV();
        }

        static void LoadFlightsCSV ()
        {
            string[] file = File.ReadAllLines("flights.csv");
            //Console.WriteLine($"");
            for (int i = 1; i < file.Length; i++)
            {
                Console.WriteLine(file[i]);
                string[] split_text = file[i].Split(",");

                string flight_no = split_text[0];
                string origin = split_text[1];
                string destination = split_text[2];
                DateTime expected_time = DateTime.Parse(split_text[3]);
                string SpecialRequestCode

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