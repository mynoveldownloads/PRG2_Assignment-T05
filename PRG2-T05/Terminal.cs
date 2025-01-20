using System;
using PRG2_T05_Flight;
using PRG2_T05_NORMFlight;
using PRG2_T05_LWTTFlight;
using PRG2_T05_Flight;
using PRG2_T05_DDJBFlight;
using PRG2_T05_CFFTFlight;

/// <summary>
/// Represents a terminal in an airport, which manages airlines and boarding gates.
/// Provides functionality to add airlines, assign boarding gates, and calculate airline fees.
/// </summary>

/// Represents a terminal in an airport, which manages airlines and boarding gates.
/// Provides functionality to add airlines, assign boarding gates, and calculate airline fees.
/// </summary>

using PRG2_T05_Flight;



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
    /// <param name="airline">The airline to add.</param>
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
    /// <param name="gate">The boarding gate to add.</param>
    /// <returns>True if the boarding gate is added successfully, false otherwise.</returns>



    /// Adds a boarding gate to the terminal if it doesn't already exist.


    /// <returns>True if the boarding gate is added successfully, false otherwise.



    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.GateName))
        {
            BoardingGates[gate.GateName] = gate;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Assigns a flight to a boarding gate.
    /// </summary>
    // <param name="gateName">The gate name.</param>


    /// <summary>
    /// Assigns a flight to a specific boarding gate.
    /// </summary>
    // <param name="gateName">The name of the gate.</param>

    // <param name="flight">The flight to assign.</param>
    /// <returns>True if the flight is assigned successfully, false otherwise.</returns>
    public bool AssignFlightToGate(string gateName, Flight flight)
    {
        if (BoardingGates.ContainsKey(gateName))
        {

            BoardingGates[gateName].AssignFlight(flight);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Clears the flight assignment from a boarding gate.
    /// </summary>
    // <param name="gateName">The gate name.</param>
    /// <returns>True if the flight assignment is cleared successfully, false otherwise.</returns>
    public bool ClearFlightFromGate(string gateName)
    {
        {

            return BoardingGates[gateName].AssignFlight(flight);
        }
        return false; // Gate does not exist
    }

    /// <summary>
    /// Clears the flight assignment from a specific boarding gate.
    /// </summary>
    // <param name="gateName">The name of the gate.</param>
    public void ClearFlightFromGate(string gateName)
    {
        if (BoardingGates.ContainsKey(gateName))
        {
            BoardingGates[gateName].ClearFlightAssignment();

            return true;
        }
        return false;

    }

}

/// <summary>
/// Retrieves the airline associated with a specific flight.
/// </summary>
// <param name="flight">The flight to find the airline for.</param>
/// <returns>The airline associated with the flight, or null if not found.</returns>




public Airline GetAirlineFromFlight(Flight flight)
{
    foreach (var airline in Airlines.Values)
    {
        if (airline.Flights.ContainsKey(flight.FlightNumber))
        {
            return airline;
        }
    }
    return null;
}




/// <summary>
/// Prints the calculated fees for all airlines in the terminal.
/// </summary>
public void PrintAirlineFees()
{
    Console.WriteLine($"--- Fees Summary for Terminal {TerminalName} ---");


    public void PrintAirlineFees()
    {

        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"{airline.Name} Fees: ${airline.CalculateFees():F2}");
        }


        double gateFees = CalculateTotalGateFees();
        Console.WriteLine($"Total Gate Fees: ${gateFees:F2}");
        Console.WriteLine($"Overall Terminal Fees: ${gateFees + Airlines.Values.Sum(a => a.CalculateFees()):F2}");
    }

    /// <summary>
    /// Calculates the total fees for all boarding gates in the terminal.
    /// </summary>
    /// <returns>The total gate fees.</returns>
    public double CalculateTotalGateFees()
    {
        double totalFees = 0;
        foreach (var gate in BoardingGates.Values)
        {
            if (gate.AssignedFlight != null) // Only calculate fees for gates with assigned flights
            {
                totalFees += gate.CalculateFees();
            }
        }
        return totalFees;

    }

    public override string ToString()
{
    return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
}
}
//test