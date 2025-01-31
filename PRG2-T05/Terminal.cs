using System;
using System.Collections.Generic;
using System.Linq;
using PRG2_T05_Flight;
using PRG2_T05_NORMFlight;

public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; private set; } = new Dictionary<string, Airline>();
    public Dictionary<string, Flight> Flights { get; private set; } = new Dictionary<string, Flight>();
    public Dictionary<string, double> GateFees { get; private set; } = new Dictionary<string, double>();
    public Dictionary<string, BoardingGate> BoardingGates { get; private set; } = new Dictionary<string, BoardingGate>();

    public Terminal(string terminalName, Dictionary<string, Airline> airline_dict, Dictionary<string, Flight> flight_dict, Dictionary<string, double> gate_fees, Dictionary<string, BoardingGate> boarding_gate_dict)
    {
    this.TerminalName = terminalName;
    this.Airlines = airline_dict;
    this.Flights = flight_dict;
    this.GateFees = gate_fees;
    this.BoardingGates = boarding_gate_dict;
    }

    // Feature 1: Adding Airlines
    public bool AddAirline(Airline airline)
    {
        if (airline == null || Airlines.ContainsKey(airline.Code))
            return false;

        Airlines[airline.Code] = airline;
        return true;
    }

    // Feature 1: Adding Boarding Gates
    public bool AddBoardingGate(BoardingGate gate)
    {
        if (gate == null || BoardingGates.ContainsKey(gate.gateName))
            return false;

        BoardingGates[gate.gateName] = gate;
        return true;
    }

    // Feature 7: Get Airline from a Flight
    public Airline GetAirlineFromFlight(Flight flight)
    {
        if (flight == null || string.IsNullOrEmpty(flight.FlightNumber) || flight.FlightNumber.Length < 2)
            return null;

        string code = flight.FlightNumber.Substring(0, 2);
        return Airlines.ContainsKey(code) ? Airlines[code] : null;
    }

    public void PrintAirlines()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-12} {1}", "Airline Code", "Airline Name");

        foreach (var airline in Airlines.Values.OrderBy(a => a.Code))
        {
            Console.WriteLine("{0,-12} {1}", airline.Code, airline.Name);
        }
    }

    public void PrintAirlineFees()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Airline Fees for Terminal 5");
        Console.WriteLine("=============================================");
        
        foreach (var airline in Airlines.Values)
        {

            double subtotal = airline.CalculateFees();
            double discounts = 0;
            int flightCount = airline.Flights.Count;
            int earlyLateFlights = 0, eligibleOriginFlights = 0, noSpecialRequestFlights = 0;

            Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");

            foreach (Flight flight in airline.Flights.Values)
            {
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                    earlyLateFlights++;

                if (new[] { "DXB", "BKK", "NRT" }.Contains(flight.Origin))
                    eligibleOriginFlights++;

                if (flight is NORMFlight)
                    noSpecialRequestFlights++;
            }

            // Applying Promotions
            discounts += (flightCount / 3) * 350;
            discounts += earlyLateFlights * 110;
            discounts += eligibleOriginFlights * 25;
            discounts += noSpecialRequestFlights * 50;

            if (flightCount > 5)
                discounts += subtotal * 0.03;

            double finalTotal = subtotal - discounts;

            Console.WriteLine($"Subtotal: ${subtotal:0.00}");
            Console.WriteLine($"Discounts: -${discounts:0.00}");
            Console.WriteLine($"Final Total: ${finalTotal:0.00}");
            Console.WriteLine("---------------------------------------------");
        }
    }

    public override string ToString() => $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Gates: {BoardingGates.Count}";
}
