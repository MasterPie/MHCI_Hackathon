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

namespace MHCI.Hackathon.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebKit.WebKitBrowser _browser;
        private IPlayerInput _playerInputEngine;

        public MainWindow()
        {
            InitializeComponent();

            //_playerInputEngine = new KinectInput();
            _playerInputEngine = new AutomatedInput();
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
                Model.Song song = new Model.Song("Song", "../../../../../../../Desktop/bells_gm");
                Model.MusicPlayer player = new Model.MusicPlayer();

                player.Play(song);

                //Console.WriteLine("Player {0} Volume {1} Craziness {2}", action.Player.Id, action.Volume, action.Craziness);
            }
        }


        #region WebKitBrowser Routines
        /// <summary>
        /// Sends javascript calls to the underlying webkit engine
        /// </summary>
        /// <param name="js">string containing javascript code</param>
        public void MakeJSCall(string js)
        {
            try
            {
                _browser.StringByEvaluatingJavaScriptFromString(js);
            }
            catch (System.Exception h)
            {
                InitializeComponent(); // Restart UI
            }
        }
        #endregion

        /// <summary>
        /// Initializes several code routines after the webkit containing element loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browserContainer_Loaded(object sender, RoutedEventArgs e)
        {
            return;
            WindowsFormsHost host = new WindowsFormsHost();

            _browser = new WebKit.WebKitBrowser();
            host.Child = _browser;

            this.browserContainer.Children.Add(host);
            
            string url = "";
            _browser.Navigate(url);

            /*
            this.ViewModel.PropertyChanged += (o, f) =>
            {
                if (f.PropertyName.Equals("URI"))
                {
                    MakeJSCall(this.ViewModel.URI);
                    //playSoundFeedback();
                }
            };*/

            //setupTimer.Start();


        }

    }
}
