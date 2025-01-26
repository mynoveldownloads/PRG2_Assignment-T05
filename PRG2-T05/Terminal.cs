//==========================================================
// Student Number : S10259207E
// Student Name   : Raphael Adesta Pratidina
// Partner Name   : Yeaw Min Lee
//==========================================================

using System; // Added missing using directive
using System.Collections.Generic;
using System.Linq; // Added for LINQ OrderBy
using PRG2_T05_Flight;

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

    public override string ToString() => $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
} // Added missing closing brace for class