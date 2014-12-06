using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using MHCI.Hackathon.App.Kinect;
using IrrKlang;

namespace MHCI.Hackathon.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private IPlayerInput _playerInputEngine;
        private Model.MusicPlayer _musicPlayer;

        public MainWindow()
        {
            InitializeComponent();

            //_playerInputEngine = new KinectInput();
            _playerInputEngine = new AutomatedInput();
            _playerInputEngine.PlayerActionsChanged += _playerInputEngine_PlayerActionsChanged;
            _playerInputEngine.PlayerJoined += _playerInputEngine_PlayerJoined;
            _playerInputEngine.PlayerLeft += _playerInputEngine_PlayerLeft;

            String relativePath = "ChristmasSong.mp3";
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = appDirectory + relativePath;
            Model.Song song = new Model.Song("Song", absolutePath);
            _musicPlayer = new Model.MusicPlayer();

            uint length = _musicPlayer.Play(song, false);
            
        }


        #region Kinect Events
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
                //String relativePath = "../../../../../../../Desktop/ChristmasSong";
                


                //MakeJSCall("acceptAction", action.Player.Id, action.Volume, action.Craziness);
                //Console.WriteLine("Player {0} Volume {1} Craziness {2}", action.Player.Id, action.Volume, action.Craziness);
            }
        }
        #endregion

        #region WebKitBrowser Routines
        /// <summary>
        /// Sends javascript calls to the underlying webkit engine
        /// </summary>
        /// <param name="js">string containing javascript code</param>
        public void MakeJSCall(string js, params object[] args)
        {
            try
            {
                browser.InvokeScript(js, args);
                //_browser.StringByEvaluatingJavaScriptFromString(js);
            }
            catch (System.Exception h)
            {
                InitializeComponent(); // Restart UI
            }
        }
        #endregion
    }
}
