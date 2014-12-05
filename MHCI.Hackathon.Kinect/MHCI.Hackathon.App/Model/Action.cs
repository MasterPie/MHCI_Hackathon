using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Hackathon.App.Model
{
    public class Action
    {
        Player Player { get; set; }
        double Volume { get; set; } // 0 - 10 ft (not sure about units)
        double Craziness { get; set; } // 0 - 10
    }
}
