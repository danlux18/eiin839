using System;

public class Station
{
	public Station()
	{
        public Double number { get; set; }
        public String contract_name { get; set; }
        public String name { get; set; }
        public String adress { get; set; }
        public Position position { get; set; }
        public Boolean banking { get; set; }
        public Boolean bonus { get; set; }
        public String status { get; set; }
        public String lastUpdate { get; set; }
        public Boolean connected { get; set; }
        public Boolean overflow { get; set; }
        public String shape { get; set; }
        public Stands totalStands { get; set; }
        public Stands mainStands { get; set; }
        public Stands overflowStands { get; set; }
        public String toString()
        {
            return "Station : " + name + ", Contract_Name : " + contract_name + ", Adress :" + adress;
        }
    }
}
