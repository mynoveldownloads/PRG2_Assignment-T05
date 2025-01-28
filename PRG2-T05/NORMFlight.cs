using System;
using PRG2_T05_Flight;

namespace PRG2_T05_NORMFlight
{
    public class NORMFlight : Flight
    {
        public NORMFlight(string flight_no, string origin, string destination, DateTime expected_time, string status)
            : base(flight_no, origin, destination, expected_time, status)
        {
        }

        public override double CalculateFees()
        {
            double fee = base.CalculateFees(); // Base fee starts at $300

            // Add fees based on origin/destination
            if (Origin == "SIN")
            {
                fee += 800; // Departing from Singapore
            }
            else if (Destination == "SIN")
            {
                fee += 500; // Arriving in Singapore
            }

            return fee; // No additional request fees for normal flights
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
