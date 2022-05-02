using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PositionInstruction
    {
        public List<Position> positions;
        public Step[] instructions;

        override
        public string ToString()
        {
            string result = "Positions : [";
            if(positions != null && positions.Count > 0)
            {
                positions.ForEach(position => result += "("+position.ToString() +") ");
            }
            result += "],\n Instructions : ";
            if (instructions != null && instructions.Length > 0)
            {
                foreach(Step step in instructions)
                {
                    result += "\n    " + step.ToString();
                }
            }

            return result;
        }
    }
}
