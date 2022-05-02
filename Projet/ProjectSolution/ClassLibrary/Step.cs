namespace ClassLibrary
{
    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public string instruction { get; set; }

        override
        public string ToString()
        {
            return "Distance : " + distance + ", Duration : "+ duration + ", Instruction : "+ instruction;
        }

    }
}