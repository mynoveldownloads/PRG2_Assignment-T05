//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T05_Flight
{
    public abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber {  get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string? Status { get; set; } = null; // edited this code for feature 5

        public Flight (string flight_no, string origin, string destination, DateTime expected_time, string status)
        {
            FlightNumber = flight_no;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expected_time;
            Status = status;
        }

        public override string ToString()
        {
            return $"Flight number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected time: {ExpectedTime}, Status: {Status}";
        }

        public virtual double CalculateFees()
        {
            return 300;
        }

        public int CompareTo(Flight other)
        {
            if (other == null) return 1;
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }
    }
}
