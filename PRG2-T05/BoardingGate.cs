// BoardingGate.cs
using PRG2_T05_Flight;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight? AssignedFlight { get; private set; }

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    /// <summary>
    /// Assigns a flight to this boarding gate.
    /// </summary>
    /// <param name="flight">The flight to assign.</param>
    public void AssignFlight(Flight flight)
    {
        AssignedFlight = flight;
    }

    /// <summary>
    /// Clears the flight assignment from this boarding gate.
    /// </summary>
    public void ClearFlightAssignment()
    {
        AssignedFlight = null;
    }

    public override string ToString()
    {
        return $"Gate: {GateName}, Supports: CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT}), Assigned Flight: {AssignedFlight?.FlightNumber ?? "None"}";
    }
}
