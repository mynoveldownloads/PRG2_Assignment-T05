//==========================================================
// Student Number : S10259207E
// Student Name   : Raphael Adesta Pratidina
// Partner Name   : Yeaw Min Lee
//==========================================================

using System; // Added missing using directive
using System.Collections.Generic;
using System.Linq; // Added for LINQ OrderBy
using PRG2_T05_Flight;
using PRG2_T05_NORMFlight;

public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; private set; } = new Dictionary<string, Airline>();
    public Dictionary<string, BoardingGate> BoardingGates { get; private set; } = new Dictionary<string, BoardingGate>();

    public Terminal(string terminalName) => TerminalName = terminalName;

    // Feature 1: Correct implementation
    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }

    // Feature 1:
    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.gateName))
        {
            BoardingGates[gate.gateName] = gate;
            return true;
        }
        return false;
    }

    // Feature 7: 
    public Airline GetAirlineFromFlight(Flight flight)
    {
        string code = flight.FlightNumber.Substring(0, 2); // flight number e.g. SQ 115, airline code is SQ -> extract the first 2 letters in flight number
        if (flight == null || !Airlines.ContainsKey(code)) // if flight is null or dictionary does not contain the key, return nothing
        {
            return null;
        }
        else
        {
            Airline airline = Airlines[code];
            return airline;
        }
    }

    public void PrintAirlines()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-12} {1}", "Airline Code", "Airline Name");

        foreach (var airline in Airlines.Values.OrderBy(a => a.Code))
        {
            Console.WriteLine("{0,-12} {1}",
                airline.Code,
                airline.Name);
        }
    }


    public void PrintAirlineFees()
    {
        Console.WriteLine("=============================================\r\nAirline Fees for Terminal 5\r\n=============================================");

        foreach (var airline in Airlines.Values)
        {
            // Subtotal: Uses existing Airline.CalculateFees()
            double subtotal = airline.CalculateFees();

            // Discounts: Calculated using existing Flight properties
            double discounts = 0;
            int flightCount = airline.Flights.Count;
            int earlyLateFlights = 0;
            int eligibleOriginFlights = 0;
            int noSpecialRequestFlights = 0;

            foreach (Flight flight in airline.Flights.Values)   
            {
                // Promotion 2: Flights before 11am or after 9pm
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                    earlyLateFlights++;

                // Promotion 3: Flights from DXB, BKK, NRT
                if (new[] { "DXB", "BKK", "NRT" }.Contains(flight.Origin))
                    eligibleOriginFlights++;

                // Promotion 4: No special request (NORMFlight)
                if (flight is NORMFlight)
                    noSpecialRequestFlights++;
            }

            // Apply promotions (stackable)
            discounts += (flightCount / 3) * 350; // Promotion 1
            discounts += earlyLateFlights * 110;  // Promotion 2
            discounts += eligibleOriginFlights * 25; // Promotion 3
            discounts += noSpecialRequestFlights * 50; // Promotion 4

            // Promotion 5: 3% off subtotal if >5 flights
            if (flightCount > 5)
                discounts += subtotal * 0.03;

            // Display results
            Console.WriteLine(
                $"Airline: {airline.Name} ({airline.Code})\n" +
                $"Subtotal: ${subtotal:0.00}\n" +
                $"Discounts: -${discounts:0.00}\n" +
                $"Final Total: ${subtotal - discounts:0.00}\n" +
                "---------------------------------------------"
            );
        }
    }

    public override string ToString() => $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
} 