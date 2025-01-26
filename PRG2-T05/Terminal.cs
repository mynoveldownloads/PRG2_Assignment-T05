//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================



// Terminal.cs
using PRG2_T05_Flight;

/// <summary>
/// Represents a terminal in an airport, which manages airlines and boarding gates.
/// Provides functionality to add airlines, assign boarding gates, and calculate airline fees.
/// </summary>
public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; private set; } = new Dictionary<string, Airline>();
    public Dictionary<string, BoardingGate> BoardingGates { get; private set; } = new Dictionary<string, BoardingGate>();

    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
    }

    /// <summary>
    /// Adds an airline to the terminal.
    /// </summary>
    /// The airline to add
    /// <returns>True if the airline is added successfully, false otherwise.</returns>
    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds a boarding gate to the terminal if it doesn't already exist.
    /// </summary>
    /// The boarding gate to add.</param>
    /// <returns>True if the boarding gate is added successfully, false otherwise.</returns>
    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.gateName))
        {
            BoardingGates[gate.gateName] = gate;
            return true;
        }
        return false;
    }


    /// <summary>
    /// Retrieves the airline associated with a specific flight.
    /// </summary>
    /// <param name="flight">The flight to find the airline for.</param>
    /// <returns>The airline associated with the flight, or null if not found.</returns>
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

    /// <summary>
    /// Prints the calculated fees for all airlines in the terminal.
    /// </summary>
    public void PrintAirlineFees()
    {
        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"{airline.Name} Fees: ${airline.CalculateFees():F2}");
        }
    }

    public override string ToString()
    {
        return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
    }
}
