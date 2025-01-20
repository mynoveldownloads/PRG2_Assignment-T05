using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG2_T05_Flight;

namespace PRG2_T05_LWTTFlight
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; } = 500;

        public LWTTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee) : base(flight_no, origin, destination, expected_time, status)
        {
            RequestFee = request_fee;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override double CalculateFees()
        {
            return 300+RequestFee;
        }
    }
}
