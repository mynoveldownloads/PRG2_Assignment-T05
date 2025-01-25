//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PRG2_T05_Flight
//{
//    public abstract class Flight
//    {
//        public string FlightNumber { get; set; }
//        public string Origin { get; set; }
//        public string Destination { get; set; }
//        public DateTime ExpectedTime { get; set; }
//        public string Status { get; set; }

//        public Flight(string flight_no, string origin, string destination, DateTime expected_time, string status)
//        {
//            FlightNumber = flight_no;
//            Origin = origin;
//            Destination = destination;
//            ExpectedTime = expected_time;
//            Status = status;
//        }

//        public override string ToString()
//        {
//            return base.ToString();
//        }

//        public abstract double CalculateFees();
//    }
//}   

//my code
public abstract class Flight  // Abstract class without abstract methods
{
    // Immutable properties
    public string FlightNumber { get; init; }
    public string Origin { get; init; }
    public string Destination { get; init; }
    public DateTime ExpectedTime { get; init; }
    public string Status { get; set; }  // Mutable status

    public Flight(string number, string origin, string dest, DateTime time, string status)
    {
        FlightNumber = number;
        Origin = origin;
        Destination = dest;
        ExpectedTime = time;
        Status = status;
    }

    // Non-abstract method with default implementation
    public virtual double CalculateFees()
    {
        // Base fee logic here
        return 300; // Boarding gate base fee from Table 6
    }

    public override string ToString() =>
        $"{FlightNumber} ({Status}) - {Origin} to {Destination} | {ExpectedTime:g}";
}