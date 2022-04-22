using System.Collections.Generic;
public class Contract
{
    public string name { get; set; }
    public string commercial_name { get; set; }
    public string country_code { get; set; }
    public List<string> cities { get; set; }
    public string toString()
    {
        string res = "Contract : " + name + ", Commercial_Name : " + commercial_name + ", Country code :" + country_code + ", Cities : ";
        if (cities != null)
        {
            foreach (var city in cities)
            {
                res += city + " ";
            }
        }
        return res;
    }

    public Contract() { }

}
