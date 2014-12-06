using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MHCI.Hackathon.App.Kinect
{
    public class AutomatedInput : IPlayerInput
    {
        private Timer _timer;

        public AutomatedInput()
        {
            this._timer = new Timer(10);
            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            
        }


        public event EventHandler<IEnumerable<Model.Action>> PlayerActionsChanged;
        public event EventHandler<int> PlayerJoined;
        public event EventHandler<int> PlayerLeft;
    }
}
