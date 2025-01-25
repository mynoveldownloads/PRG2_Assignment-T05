using PRG2_T05_Flight;
using System.Collections.Generic;

public class Terminal
{
    // Fixed property names to match class diagram
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; private set; } = new();
    public Dictionary<string, BoardingGate> BoardingGates { get; private set; } = new();
    public Dictionary<string, Flight> Flights { get; private set; } = new(); // Added missing collection
    public Dictionary<string, double> GateFees { get; private set; } = new(); // From class diagram

    public Terminal(string terminalName) => TerminalName = terminalName;

    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines.Add(airline.Code, airline);
            return true;
        }
        return false;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        // Fixed property name to PascalCase
        if (!BoardingGates.ContainsKey(gate.GateName))
        {
            BoardingGates.Add(gate.GateName, gate);
            return true;
        }
        return false;
    }

    public Airline? GetAirlineFromFlight(Flight flight)
    {
        if (flight == null) return null;
        string code = flight.FlightNumber[..2];
        return Airlines.GetValueOrDefault(code);
    }

    // Enhanced fee calculation foundation
    public void PrintAirlineFees()
    {
        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"{airline.Name} Fees: ${airline.CalculateTotalFees():F2}");
        }
    }

    public override string ToString() =>
        $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
}