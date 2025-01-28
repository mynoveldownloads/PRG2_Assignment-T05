using System;
using PRG2_T05_Flight;

namespace PRG2_T05_LWTTFlight
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; } = 500;

        // Base constructor
        public LWTTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee)
            : base(flight_no, origin, destination, expected_time, status)
        {
            RequestFee = request_fee;
        }

        // Overloaded constructor without request_fee
        public LWTTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status)
            : base(flight_no, origin, destination, expected_time, status)
        {
        }

        public override string ToString()
        {
            return base.ToString() + $", Request Fee: {RequestFee}";
        }

        public override double CalculateFees()
        {
            double fee = base.CalculateFees(); // Start with base logic from parent class

            // Add LWTT-specific request fee
            fee += RequestFee;

            return fee;
        }
    }
}
