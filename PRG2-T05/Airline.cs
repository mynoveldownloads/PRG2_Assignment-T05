public class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; private set; } = new Dictionary<string, Flight>();

    public Airline(string code, string name)
    {
        Code = code;
        Name = name;
    }

    // Calculate total fees by summing fees from all flights
    public double CalculateTotalFees()
    {
        double total = 0;
        foreach (var flight in Flights.Values)
        {
            total += flight.CalculateFees();
        }
        return total;
    }
}