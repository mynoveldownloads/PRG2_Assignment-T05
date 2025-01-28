public abstract class Flight : IComparable<Flight>
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string? Status { get; set; } = null; // edited this code for feature 5
    public bool BoardingGateAssigned { get; set; } = false; // New property for gate validation
    public string? SpecialRequestCode { get; set; } = null; // For additional fee logic

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
        return $"Flight number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, Expected time: {ExpectedTime}, Status: {Status}";
    }

    public virtual double CalculateFees()
    {
        double fee = 300; // Base fee

        // Apply fee based on origin/destination
        if (Origin == "SIN" || Destination == "SIN")
        {
            fee += 800;
        }
        else
        {
            fee += 500;
        }

        // Apply additional fees for special requests
        if (!string.IsNullOrEmpty(SpecialRequestCode))
        {
            // Example: Use SpecialRequestCode to determine additional fee
            fee += GetSpecialRequestFee(SpecialRequestCode);
        }

        return fee;
    }

    protected virtual double GetSpecialRequestFee(string requestCode)
    {
        // Placeholder for additional fee logic
        return 0;
    }

    public bool ValidateBoardingGate()
    {
        return BoardingGateAssigned;
    }

    public int CompareTo(Flight other)
    {
        if (other == null) return 1;
        return this.ExpectedTime.CompareTo(other.ExpectedTime);
    }
}


