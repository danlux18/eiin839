using System;
using System.Globalization;

public class Position
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public Position() { }

    override
    public string ToString()
    {
        return longitude.ToString(CultureInfo.InvariantCulture) + "," + latitude.ToString(CultureInfo.InvariantCulture);
    }
}
