using System;
using PRG2_T05_Flight;
using PRG2_T05_NORMFlight;
using PRG2_T05_LWTTFlight;

using PRG2_T05_DDJBFlight;
using PRG2_T05_CFFTFlight;

/// <summary>
/// Represents a terminal in an airport, which manages airlines and boarding gates.
/// Provides functionality to add airlines, assign boarding gates, and calculate airline fees.
/// </summary>

/// Represents a terminal in an airport, which manages airlines and boarding gates.
/// Provides functionality to add airlines, assign boarding gates, and calculate airline fees.
/// </summary>




public class Terminal
{
    public string terminalName { get; set; }
    public Dictionary<string, Airline> airlines { get; private set; } = new Dictionary<string, Airline>();
    public Dictionary<string, BoardingGate> boardingGates { get; private set; } = new Dictionary<string, BoardingGate>();
    public Dictionary<string, double> gateFees { get; private set; } = new Dictionary<string, double>();

    public Terminal(string terminal_name, Dictionary<string, double> gate_fees)
    {
        terminalName = terminal_name;
        gateFees = gate_fees;
    }

    /*
     * for the airlines dictionary, key of the dictionary should be the airline code
     */
    public bool AddAirline(Airline airline)
    {
        if (!airlines.ContainsKey(airline.Code))
        {
            airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }


    /*
     * same logic as AddAirline method, with gateName as the key
     */
    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!boardingGates.ContainsKey(gate.gateName))
        {
            boardingGates[gate.gateName] = gate;
            return true;
        }
        return false;
    }


    // obtain airline from flight -> how does this work? whats the purpose of doing this?
    public Airline GetAirlineFromFlight(Flight flight)
    {
        string code = flight.FlightNumber.Substring(0, 2); // flight number e.g. SQ 115, airline code is SQ -> extract the first 2 letters in flight number
        if (flight == null || !airlines.ContainsKey(code)) // if flight is null or dictionary does not contain the key, return nothing
        {
            return null;
        }
        else
        {
            Airline airline = airlines[code];
            return airline;
        }
    }

    // not sure how to do this
    public void PrintAirlineFees()
    {

    }

    public override string ToString()
    {
        return base.ToString();
    }
}


/*
 * class contents:
 * attributes: terminalName, airlines, flights, boardingGates, gateFees
 * 
 * methods: AddAirline, AddBoardingGate, GetAirlineFromFlight, PrintAirlineFees, ToString
 * 
 * 
 */