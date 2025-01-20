using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG2_T05_Flight;


namespace PRG2_T05_CFFTFlight
{
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; } = 150;

        public CFFTFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee) : base(flight_no, origin, destination, expected_time, status)
        {
            RequestFee = request_fee;
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

