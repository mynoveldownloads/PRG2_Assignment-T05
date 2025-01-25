//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PRG2_T05_Flight;

//namespace PRG2_T05_DDJBFlight
//{
//    public class DDJBFlight : Flight
//    {
//        public double RequestFee { get; set; } = 300;

//        // base constructor
//        public DDJBFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee) : base(flight_no, origin, destination, expected_time, status)
//        {
//            RequestFee = request_fee;
//        }

//        // overloaded constructor to exclude request_fee as argument
//        public DDJBFlight(string flight_no, string origin, string destination, DateTime expected_time, string status) : base(flight_no, origin, destination, expected_time, status)
//        {

//        }

//        public override string ToString()
//        {
//            return base.ToString();
//        }

//        public override double CalculateFees()
//        {
//            RequestFee += 300;
//            return RequestFee;
//        }
//    }
//}



//my code

namespace PRG2_T05_Flight
{
    public class DDJBFlight : Flight
    {
        public DDJBFlight(string number, string origin, string destination, DateTime expectedTime)
            : base(number, origin, destination, expectedTime, "On Time") { }

        public override double CalculateFees()
        {
            double baseFee = Destination == "SIN" ? 500 : 800;
            return baseFee + 300 + base.CalculateFees(); // DDJB fee + Boarding Gate fee
        }
    }
}