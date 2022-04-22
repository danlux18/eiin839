using System;

public class Contract
{
	public Contract()
	{
        public String name { get; set; }
        public String commercial_name { get; set; }
        public String country_code { get; set; }
        public List<String> cities { get; set; }
        public String toString()
        {
            String res = "Contract : " + name + ", Commercial_Name : " + commercial_name + ", Country code :" + country_code + ", Cities : ";
            if (cities != null)
            {
                foreach (var city in cities)
                {
                    res += city + " ";
                }
            }
            return res;
        }
    }
}
