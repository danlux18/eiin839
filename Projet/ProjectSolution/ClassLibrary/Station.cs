using System;
public class Station
{
    public double number { get; set; }
    public string contractName { get; set; }
    public string name { get; set; }
    public Position position { get; set; }
    public string status { get; set; }
    public bool connected { get; set; }
    public Stands mainStands { get; set; }
    public string ToString()
    {
        return "Name : "+name+", contract : "+ contractName+", bikes availables : "+ mainStands.availabilities.bikes;
    }
    public Station() { }
}