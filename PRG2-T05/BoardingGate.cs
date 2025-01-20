// BoardingGate.cs
using PRG2_T05_Flight;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight AssignedFlight { get; set; }

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    public double CalculateFees()
    {
        double baseFee = 300;

        if (AssignedFlight != null)
        {
            if (SupportsDDJB) baseFee += 300;
            if (SupportsCFFT) baseFee += 150;
            if (SupportsLWTT) baseFee += 500;
        }

        return baseFee;
    }

    public override string ToString()
    {
        return $"Gate: {GateName}, Supports Special Requests: CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT})";
    }
}