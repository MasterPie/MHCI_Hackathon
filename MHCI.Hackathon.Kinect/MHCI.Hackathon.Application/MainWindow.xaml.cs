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
using System.IO;

namespace MHCI.Hackathon.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private IPlayerInput _playerInputEngine;
        //private Model.MusicPlayer _musicPlayer;
        private ISound _sound_layer1;
        private ISound _sound_layer2;
        private ISound _sound_layer3;
        private ISound _sound_layer4;


        Model.Song _song_layer1;
        Model.Song _song_layer2;
        Model.Song _song_layer3;
        Model.Song _song_layer4;

        private Boolean isPlaying;

        private ISoundEngine _engine;

        public MainWindow()
        {
            InitializeComponent();

            _playerInputEngine = new KinectInput();
            //_playerInputEngine = new AutomatedInput();
            _playerInputEngine.PlayerActionsChanged += _playerInputEngine_PlayerActionsChanged;
            _playerInputEngine.PlayerJoined += _playerInputEngine_PlayerJoined;
            _playerInputEngine.PlayerLeft += _playerInputEngine_PlayerLeft;

            _engine = new ISoundEngine();

            //browser.Navigate("");
            browser.Navigate("http://localhost:3001");
            browser.Navigated += browser_Navigated;

            //_song_layer1 = new Model.Song("Rhythm", "Rhythm.mp3");
            _song_layer1 = new Model.Song("Vocals", "Vocals.mp3");
            _song_layer2 = new Model.Song("Organs", "Organs.mp3");
            _song_layer3 = new Model.Song("Keyboards", "Keyboards.mp3");
            _song_layer4 = new Model.Song("Vocals", "Vocals.mp3");

        }

        #region Kinect Events
        void _playerInputEngine_PlayerLeft(object sender, int e)
        {
            switch (e)
            {
                case 1:
                    _sound_layer1.Volume = 0;
                    _sound_layer1.PlaybackSpeed = 1f;
                    break;
                case 2:
                    _sound_layer2.Volume = 0f;
                    _sound_layer2.PlaybackSpeed = 1f;
                    break;
                case 3:
                    _sound_layer3.Volume = 0f;
                   _sound_layer3.PlaybackSpeed = 1f;
                    break;
                case 4:
                    _sound_layer4.Volume = 0f;
                    _sound_layer4.PlaybackSpeed = 1f;
                    break;
                default:
                    break;
            }
            //Console.WriteLine("Player left: " + e);
            MakeJSCall("playerLeft", e);
        }

        void _playerInputEngine_PlayerJoined(object sender, int e)
        {
            if (!isPlaying)
            {
                _sound_layer1 = this._engine.Play2D(_song_layer1.FileLocation, false);

                _sound_layer2 = this._engine.Play2D(_song_layer2.FileLocation, false);
                _sound_layer2.Volume = 0;

                _sound_layer3 = this._engine.Play2D(_song_layer3.FileLocation, false);
                _sound_layer3.Volume = 0;

                _sound_layer4 = this._engine.Play2D(_song_layer4.FileLocation, false);
                _sound_layer4.Volume = 0;

                isPlaying = true;
            }
            else
            {
                switch (e)
                {
                    case 1:
                        _sound_layer1.Volume = 1;
                        _sound_layer1.PlaybackSpeed = 1.0f;
                        break;
                    case 2:
                        _sound_layer2.Volume = 1;
                        _sound_layer2.PlaybackSpeed = 1.0f;
                        break;
                    case 3:
                        _sound_layer3.Volume = 1;
                        _sound_layer3.PlaybackSpeed = 1.0f;
                        break;
                    case 4:
                        _sound_layer4.Volume = 1;
                        _sound_layer4.PlaybackSpeed = 1.0f;
                        break;
                    default:
                        break;
                }
            }
            //throw new NotImplementedException();
        }

        void _playerInputEngine_PlayerActionsChanged(object sender, IEnumerable<Model.Action> e)
        {
            foreach (var action in e)
            {
                switch (action.Player.Id)
                {
                    case 1:
                        _sound_layer1.Volume = setVolume(action.Volume);
                        _sound_layer1.PlaybackSpeed = setPlaybackRate(action.Craziness);
                        break;
                    case 2:
                        _sound_layer2.Volume = setVolume(action.Volume);
                        _sound_layer2.PlaybackSpeed = setPlaybackRate(action.Craziness);
                        break;
                    case 3:
                        _sound_layer3.Volume = setVolume(action.Volume);
                        _sound_layer3.PlaybackSpeed = setPlaybackRate(action.Craziness);
                        break;
                    case 4:
                        _sound_layer4.Volume = setVolume(action.Volume);
                        _sound_layer4.PlaybackSpeed = setPlaybackRate(action.Craziness);
                        break;
                    default:
                        break;    
                }

                

                MakeJSCall("acceptAction", action.Player.Id, action.Volume, action.Craziness);
                Console.WriteLine("Player {0} Volume {1} Craziness {2}", action.Player.Id, action.Volume, action.Craziness);
            }
        }
        #endregion

        private float setPlaybackRate(double playbackRate)
        {
            int playbackRateint = (int) playbackRate;
            float newPlaybackRate = 1;
            switch(playbackRateint)
            {
                case 0:
                    newPlaybackRate = 0.5f;
                    break;
                case 1:
                    newPlaybackRate = 0.5f;
                    break; 
                case 2:
                    newPlaybackRate = 0.5f;
                    break;
                case 3:
                    newPlaybackRate = 0.5f;
                    break;
                case 4:
                    newPlaybackRate = 0.5f;
                    break;
                case 5:
                    newPlaybackRate = 0.5f;
                    break;
                case 6:
                    newPlaybackRate = 0.5f;
                    break;
                case 7:
                    newPlaybackRate = 1.0f;
                    break;
                case 8:
                    newPlaybackRate = 1.1f;
                    break;
                case 9:
                    newPlaybackRate = 1.4f;
                    break;
                case 10:
                    newPlaybackRate = 1.4f;
                    break;
                default:
                    break;
            }
            return newPlaybackRate;
        }

        private float setVolume(double volume)
        {
            int volumeInt = (int)volume;
            float newVolume = 1;
            switch (volumeInt)
            {
                case 0:
                    newVolume = 0.1f;
                    break;
                case 1:
                    newVolume = 0.1f;
                    break;
                case 2:
                    newVolume = 0.7f;
                    break;
                case 3:
                    newVolume = 0.8f;
                    break;
                case 4:
                    newVolume = 0.9f;
                    break;
                case 5:
                    newVolume = 1.0f;
                    break;
                case 6:
                    newVolume = 1.1f;
                    break;
                case 7:
                    newVolume = 1.2f;
                    break;
                case 8:
                    newVolume = 1.3f;
                    break;
                case 9:
                    newVolume = 1.4f;
                    break;
                case 10:
                    newVolume = 1.5f;
                    break;
                default:
                    break;
            }
            return newVolume;
        }

        #region WebKitBrowser Routines

        bool canMakeCalls = false;
        void browser_Navigated(object sender, NavigationEventArgs e)
        {
            canMakeCalls = true;
        }


        /// <summary>
        /// Sends javascript calls to the underlying webkit engine
        /// </summary>
        /// <param name="js">string containing javascript code</param>
        public void MakeJSCall(string js, params object[] args)
        {
            if (!canMakeCalls)
                return;

            try
            {
                browser.InvokeScript(js, args);
                //_browser.StringByEvaluatingJavaScriptFromString(js);
            }
            catch (System.Exception h)
            {
                //InitializeComponent(); // Restart UI
            }
        }
        #endregion
    }
}
