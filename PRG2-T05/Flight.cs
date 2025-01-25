using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T05_Flight
{
    public abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight(string flight_no, string origin, string destination, DateTime expected_time, string status)
        {
            FlightNumber = flight_no;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expected_time;
            Status = status;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public abstract double CalculateFees();
    }
}   