using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ResultObject
    {
        public bool worthIt { get; set; }
        public Position startPosition { get; set; }
        public PositionInstruction footToStation { get; set; }
        public PositionInstruction stationToSation { get; set; }
        public PositionInstruction sationToFoot { get; set; }
        public PositionInstruction footToFoot { get; set; }

        override
        public string ToString()
        {
            return "WorthIt : " + worthIt +
                   ",\n StartPosition : " + startPosition +
                   ",\n FootToStation : " + footToStation +
                   ",\n StationToSation : " + stationToSation +
                   ",\n SationToFoot : " + sationToFoot +
                   ",\n FootToFoot : " + startPosition;
        }
    }
}
