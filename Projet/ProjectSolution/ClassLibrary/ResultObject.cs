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
    }
}
