using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Hackathon.App.Kinect
{
    public interface IPlayerInput
    {
        event EventHandler<IEnumerable<Model.Action>> PlayerActionsChanged;
        event EventHandler<int> PlayerJoined;
        event EventHandler<int> PlayerLeft;
    }
}
