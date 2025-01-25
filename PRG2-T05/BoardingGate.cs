using PRG2_T05_Flight;

public class BoardingGate
{
    // Fixed property names to PascalCase
    public string GateName { get; init; }
    public bool SupportsCFFT { get; init; }
    public bool SupportsDDJB { get; init; }
    public bool SupportsLWTT { get; init; }
    public Flight? AssignedFlight { get; private set; }

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    public bool AssignFlight(Flight flight)
    {
        if (AssignedFlight != null) return false;
        AssignedFlight = flight;
        return true;
    }

    public void ClearAssignment() => AssignedFlight = null;

    // Added fee calculation foundation
    public double CalculateUsageFee() => 300; // Base fee from Table 6

    public override string ToString() =>
        $"{GateName} (CFFT: {SupportsCFFT}, DDJB: {SupportsDDJB}, LWTT: {SupportsLWTT})";
}