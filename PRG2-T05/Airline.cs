//==========================================================
// Student Number	: S10259207E
// Student Name	: Raphael Adesta Pratidina
// Partner Name	: Yeaw Min Lee
//==========================================================


// Airline.cs

using PRG2_T05_Flight;
using PRG2_T05_NORMFlight;
using System;
using System.Collections.Generic;

public class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; private set; } = new Dictionary<string, Flight>();

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public bool AddFlight(Flight flight)
    {
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights[flight.FlightNumber] = flight;
            return true;
        }
        return false;
    }

    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }

    public double CalculateFees()
    {
        double subtotal = 0;
        foreach (var flight in Flights.Values)
        {
            subtotal += flight.CalculateFees(); // Uses Flight's CalculateFees()
        }
        return subtotal; // Subtotal before discounts
    }

    public override string ToString()
    {
        return $"Airline: {Name} ({Code}), Total Flights: {Flights.Count}";
    }
}