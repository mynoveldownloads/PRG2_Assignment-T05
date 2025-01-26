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

        // base constructor
        public LWTTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee) : base(flight_no, origin, destination, expected_time, status)
        {
            RequestFee = request_fee;
        }

        // overloaded constructor without request_fee
        public LWTTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status) : base(flight_no, origin, destination, expected_time, status)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override double CalculateFees()
        {
            RequestFee += 300;
            return RequestFee;
        }
    }
}