using System;
public class Station
{
    public double number { get; set; }
    public string contractName { get; set; }
    public string name { get; set; }
    public Position position { get; set; }
    public string status { get; set; }
    public bool connected { get; set; }
    public Stands totalStands { get; set; }
    public string toString()
    {
        return "Station : " + name + ", Contract_Name : " + contractName;
    }
    public Station() { }
}