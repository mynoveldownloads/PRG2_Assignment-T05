//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


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
            return base.CalculateFees(); // No special request fee
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
