using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG2_T05_Flight;

namespace PRG2_T05_NORMFlight
{
    public class NORMFlight : Flight
    {

        public NORMFlight(string flight_no, string origin, string destination, DateTime expected_time, string status) : base (flight_no, origin, destination, expected_time, status) 
        {

        }

        public override double CalculateFees()
        {
            return 0.0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
