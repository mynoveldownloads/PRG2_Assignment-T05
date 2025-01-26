//==========================================================
// Student Number : S10259207E
// Student Name   : Raphael Adesta Pratidina
// Partner Name   : Yeaw Min Lee
//==========================================================

using PRG2_T05_Flight;
using System.Collections.Generic;

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
        string code = flight.FlightNumber.Substring(0, 2);
        return Airlines.ContainsKey(code) ? Airlines[code] : null;
    }


    public override string ToString() => $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
}