﻿//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


using System;
using PRG2_T05_Flight;

namespace PRG2_T05_DDJBFlight
{
    public class DDJBFlight : Flight
    {
        public double RequestFee { get; set; } = 300;

        // Base constructor
        public DDJBFlight(string flight_no, string origin, string destination, DateTime expected_time, string status, double request_fee)
            : base(flight_no, origin, destination, expected_time, status)
        {
            RequestFee = request_fee;
        }

        // Overloaded constructor to exclude request_fee as an argument
        public DDJBFlight(string flight_no, string origin, string destination, DateTime expected_time, string status)
            : base(flight_no, origin, destination, expected_time, status)
        {
        }

        public override string ToString()
        {
            return base.ToString() + $", Request Fee: {RequestFee}";
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee; // Adds DDJB fee (300)
        }
    }
}