using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Terminal
{
    // Public properties with private setters (C# conventions)
    public string TerminalName { get; private set; }
    public Dictionary<string, Airline> Airlines { get; private set; } = new Dictionary<string, Airline>();
    public Dictionary<string, Flight> Flights { get; private set; } = new Dictionary<string, Flight>();
    public Dictionary<string, BoardingGate> BoardingGates { get; private set; } = new Dictionary<string, BoardingGate>();
    public Dictionary<string, double> GateFees { get; private set; } = new Dictionary<string, double>();

    // Constructor
    public Terminal(string terminalName) => TerminalName = terminalName;

    // Class diagram methods
    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.gateName))
        {
            BoardingGates[gate.gateName] = gate;
            GateFees[gate.gateName] = 300; // Base fee from Table 6
            return true;
        }
        return false;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        if (flight == null) return null;
        var airlineCode = flight.FlightNumber.Split(' ')[0];
        return Airlines.TryGetValue(airlineCode, out Airline airline) ? airline : null;
    }

    public void PrintAirlineFees()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine($"Airline Fee Report for {TerminalName}");
        Console.WriteLine("=============================================");

        // Only apply Tables 6 & 7 to Terminal 5
        if (TerminalName != "Terminal 5")
        {
            Console.WriteLine("\nTables 6 & 7 apply only to Terminal 5.");
            return;
        }

        foreach (var airline in Airlines.Values)
        {
            var airlineFlights = Flights.Values
                .Where(f => GetAirlineFromFlight(f)?.Code == airline.Code)
                .ToList();

            if (!airlineFlights.Any()) continue;

            double baseFees = 0;
            double discounts = 0;
            int earlyLateCount = 0;
            int originDiscountCount = 0;
            int noSpecialRequestCount = 0;

            // Calculate base fees and track discounts (Table 6 & 7)
            foreach (var flight in airlineFlights)
            {
                // Table 6: Base fees
                double flightFee = flight.Destination == "Singapore (SIN)" ? 500 : 800;
                flightFee += 300; // Boarding gate base fee

                // Table 6: Special request fees
                flightFee += flight.SpecialRequestCode switch
                {
                    "DDJB" => 300,
                    "CFFT" => 150,
                    "LWTT" => 500,
                    _ => 0
                };

                baseFees += flightFee;

                // Table 7: Discount qualifications
                var time = DateTime.ParseExact(flight.ExpectedTime.Replace(".", ""),
                    "h:mmtt",
                    CultureInfo.InvariantCulture).TimeOfDay;

                if (time < TimeSpan.FromHours(11) || time >= TimeSpan.FromHours(21))
                    earlyLateCount++;

                if (new[] { "Dubai (DXB)", "Bangkok (BKK)", "Tokyo (NRT)" }.Contains(flight.Origin))
                    originDiscountCount++;

                if (string.IsNullOrEmpty(flight.SpecialRequestCode))
                    noSpecialRequestCount++;
            }

            // Table 7: Calculate discounts
            discounts += (airlineFlights.Count / 3) * 350;    // Every 3 flights
            discounts += earlyLateCount * 110;                // Early/late flights
            discounts += originDiscountCount * 25;            // Eligible origins
            discounts += noSpecialRequestCount * 50;          // No special requests

            if (airlineFlights.Count > 5)
                discounts += baseFees * 0.03;                 // Bulk discount

            // Display results
            Console.WriteLine("\n---------------------------------------------");
            Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");
            Console.WriteLine($"Total Flights: {airlineFlights.Count}");
            Console.WriteLine($"Base Fees: {baseFees:C}");
            Console.WriteLine($"Discounts: -{discounts:C}");
            Console.WriteLine($"Final Amount Due: {baseFees - discounts:C}");
        }
    }

    public double CalculateFees(List<Flight> flights)
    {
        if (TerminalName != "Terminal 5")
            throw new InvalidOperationException("Tables 6/7 apply only to Terminal 5");

        double total = 0;
        int earlyLate = 0, originDiscount = 0, noRequest = 0;

        foreach (var flight in flights)
        {
            // Table 6 Fees
            total += flight.Destination == "Singapore (SIN)" ? 500 : 800;
            total += flight.SpecialRequestCode switch
            {
                "DDJB" => 300,
                "CFFT" => 150,
                "LWTT" => 500,
                _ => 0
            };

            // Table 7 Discount Qualifications
            var time = DateTime.ParseExact(flight.ExpectedTime.Replace(".", ""),
                "h:mmtt",
                System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;

            if (time < TimeSpan.FromHours(11) || time >= TimeSpan.FromHours(21)) earlyLate++;
            if (new[] { "Dubai (DXB)", "Bangkok (BKK)", "Tokyo (NRT)" }.Contains(flight.Origin)) originDiscount++;
            if (string.IsNullOrEmpty(flight.SpecialRequestCode)) noRequest++;
        }

        // Table 7 Discounts
        double discounts = (flights.Count / 3) * 350;
        discounts += earlyLate * 110;
        discounts += originDiscount * 25;
        discounts += noRequest * 50;
        if (flights.Count > 5) discounts += total * 0.03;

        return total - discounts;
    }

    public override string ToString() =>
        $"{TerminalName} (Airlines: {Airlines.Count}, Flights: {Flights.Count}, Gates: {BoardingGates.Count})";
}
