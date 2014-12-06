using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Hackathon.App.Model
{
    public class Action
    {
        public Player Player { get; set; }
        public double Volume { get; set; } // 0 - 10 ft (not sure about units)
        public double Craziness { get; set; } // 0 - 10
    }
}
