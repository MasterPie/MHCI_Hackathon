using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace MHCI.Hackathon.App.Kinect
{
    public class KinectInput : IPlayerInput
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        Dictionary<int, Model.Action> _gameState;
        Dictionary<int, CameraSpacePoint> _pastPosition;

        Dictionary<int, int> _playerMap;

        public KinectInput()
        {
            _sensor = KinectSensor.GetDefault();
            _gameState = new Dictionary<int, Model.Action>();
            _pastPosition = new Dictionary<int, CameraSpacePoint>();
            _playerMap = new Dictionary<int, int>();

            if (_sensor != null)
            {
                _sensor.Open();
            }

            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Infrared |
                                             FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += _reader_MultiSourceFrameArrived;
        }

        #region Handle Input
        void _reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            HandleBody(body);
                        }
                    }
                }
            }

            var actions = this._gameState.Select(state => state.Value);

            if (this.PlayerActionsChanged != null)
            {
                this.PlayerActionsChanged(this, actions);
            }
        }
        private void HandleBody(Body body)
        {
            if (body == null)
                return;

            if (!body.IsTracked)
                return;

            if (RemovePlayerIfOutOfBounds(body))
                return;
            
            var spine = body.Joints.Single(j => j.Key == JointType.SpineMid).Value;

            if (!IsPlaying(spine.Position))
                return;

            //Check if new
            int playerId = AddPlayerIfNewOrGetExistingId(body);

            var volumeNumber = TransformToAppVolume(spine.Position);
            var beatNumber = TransformToAppBeats(playerId,spine.Position);

            Model.Action action = new Model.Action()
            {
                Player = new Model.Player() { Id = playerId},
                Craziness = beatNumber,
                Volume = volumeNumber
            };


            if (!this._gameState.ContainsKey(playerId))
                this._gameState.Add(playerId, action);
            else
                this._gameState[playerId] = action;
        }

        private bool IsPlaying(CameraSpacePoint bodyPosition)
        {
            var spineZPosition = bodyPosition.Z;
            var xPosition = bodyPosition.X;
            var yPosition = bodyPosition.Y;
            //Console.WriteLine(xPosition);
            return spineZPosition > 1.55 && spineZPosition < 4.5 && xPosition > -1.2 && xPosition < 1.2;
        }
        #endregion

        #region Add / Remove players

        private int AddPlayerIfNewOrGetExistingId(Body body)
        {
            int playerId = (int)body.TrackingId;

            if (this._playerMap.ContainsKey(playerId))
                return this._playerMap[playerId];
            else
            {
                var appPlayerids = this._playerMap.Values;
                int idToCreate = appPlayerids.Contains(1) ? appPlayerids.Contains(2) ? appPlayerids.Contains(3) ? 4 : 3 : 2 : 1;

                if (!this._playerMap.ContainsKey(playerId))
                    this._playerMap[playerId] = -1;

                this._playerMap[playerId] = idToCreate;
                Console.WriteLine("[{0}] Added new player {1} at {2}",playerId, idToCreate, DateTime.Now.ToShortTimeString());

                if (PlayerJoined != null)
                {
                    this.PlayerJoined(this, idToCreate);
                }

                return idToCreate;
            }
        }

        private bool RemovePlayerIfOutOfBounds(Body body)
        {
            int playerId = (int)body.TrackingId;
            var xPosition = body.Joints.Single(j => j.Key == JointType.SpineMid).Value.Position;

            if (IsPlaying(xPosition))
                return false;

            if (this._playerMap.ContainsKey(playerId))
            {
                int id = this._playerMap[playerId];
                this._playerMap.Remove(playerId);
                Console.WriteLine("[{0}] Removed player {1} at time {2}", playerId, id, DateTime.Now.ToShortTimeString());

                if (PlayerLeft != null)
                {
                    this.PlayerLeft(this, id);
                }

                return true;
            }
            return false;
        }

        #endregion

        #region Transformations

        private double TransformToAppVolume(CameraSpacePoint bodyPosition)
        {
            var spineZPosition = bodyPosition.Z * 3.28084; //convert to feet
            var volume = Math.Abs(((int)spineZPosition - 1) - 10);
            Console.WriteLine("VOLUME {0}", volume);
            return Math.Abs(volume);
        }

        private double TransformToAppBeats(int id, CameraSpacePoint bodyPosition)
        {
            var spineZPosition = bodyPosition.Z;

            if (this._pastPosition.ContainsKey(id))
            {
                var pastPosition = _pastPosition[id];

                var xDist = Math.Abs(pastPosition.X - bodyPosition.X);
                var yDist = Math.Abs(pastPosition.Y - bodyPosition.Y);
                var zDist = Math.Abs(pastPosition.Z - bodyPosition.Z);

                var totalDist = xDist + yDist + zDist;
                totalDist = totalDist * 3.28084F;
                //min 1.3
                //max 2
                // y = 1 + (x-A)*(10-1)/(B-A)
                //Console.WriteLine(totalDist * 100);
                var scaledDist = (totalDist * 100 - 5)* 1.5;

                if (scaledDist > 10)
                    scaledDist = 10;
                if (scaledDist < 0)
                    scaledDist = 0;

                this._pastPosition[id] = bodyPosition;
                Console.WriteLine("VELOCITY {0}", scaledDist);
                return scaledDist;
            }
            else
            {
                this._pastPosition.Add(id, bodyPosition);
                return 5;
            }
        }
        #endregion

        public event EventHandler<IEnumerable<Model.Action>> PlayerActionsChanged;
        public event EventHandler<int> PlayerJoined;
        public event EventHandler<int> PlayerLeft;
    }
}
