//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


// BoardingGate.cs
using PRG2_T05_Flight;

public class BoardingGate
{
    public string gateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight? AssignedFlight { get;  set; } // removed private, cant be accessed for option 3

    public BoardingGate(string GateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        gateName = GateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    public double CalculateFees()
    {
        return 0;
    }

    public override string ToString()
    {
        return $"Gate: {gateName}, Supports: CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT}), Assigned Flight: {AssignedFlight?.FlightNumber ?? "None"}";
    }
}