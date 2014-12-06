using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MHCI.Hackathon.App.Kinect;

namespace MHCI.Hackathon.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IPlayerInput _playerInputEngine;

        public App()
        {
            _playerInputEngine = new KinectInput();
            _playerInputEngine.PlayerActionsChanged += _playerInputEngine_PlayerActionsChanged;
            _playerInputEngine.PlayerJoined += _playerInputEngine_PlayerJoined;
            _playerInputEngine.PlayerLeft += _playerInputEngine_PlayerLeft;
        }

        void _playerInputEngine_PlayerLeft(object sender, int e)
        {
            //throw new NotImplementedException();
        }

        void _playerInputEngine_PlayerJoined(object sender, int e)
        {
            //throw new NotImplementedException();
        }

        void _playerInputEngine_PlayerActionsChanged(object sender, IEnumerable<Model.Action> e)
        {
            foreach (var action in e)
            {
                //Console.WriteLine("Player {0} Volume {1} Craziness {2}", action.Player.Id, action.Volume, action.Craziness);
            }
        }
    }
}
