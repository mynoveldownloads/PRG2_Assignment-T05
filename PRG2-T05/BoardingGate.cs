// BoardingGate.cs
namespace PRG2_T05_Flight
{
    public class BoardingGate
    {
        public string gateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight AssignedFlight { get; set; } // Removed nullable

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            this.gateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        // Added CalculateFees() method
        public double CalculateFees()
        {
            // Base fee for all boarding gates (Table 6: Boarding Gate Base Fee = $300)
            return 300.0;
        }

        // ToString() method to display gate details
        public override string ToString()
        {
            return $"Gate: {gateName}, Supports: CFFT({SupportsCFFT}), DDJB({SupportsDDJB}), LWTT({SupportsLWTT}), Assigned Flight: {AssignedFlight?.FlightNumber ?? "None"}";
        }
    }
}