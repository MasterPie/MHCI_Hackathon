using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace MHCI.Hackathon.App.Kinect
{
    public class KinectInput
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        Dictionary<int, Model.Action> _gameState;
        Dictionary<int, CameraSpacePoint> _pastPosition;

        public KinectInput()
        {
            _sensor = KinectSensor.GetDefault();
            _gameState = new Dictionary<int, Model.Action>();

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

            var spine = body.Joints.Single(j => j.Key == JointType.SpineMid).Value;
            int playerId = (int)body.TrackingId;

            if (!IsPlaying(spine.Position))
                return;

            var volumeNumber = TransformToAppVolume(spine.Position);
            var beatNumber = TransformToAppBeats(playerId,spine.Position);

            Model.Action action = new Model.Action()
            {
                Player = new Model.Player() { Id = playerId},
                Craziness = beatNumber,
                Volume = volumeNumber
            };


            if (!this._gameState.ContainsKey(playerId))
            {
                this._gameState.Add(playerId, action);
            }
            else
            {
                this._gameState[playerId] = action;
            }
        }


        private bool IsPlaying(CameraSpacePoint bodyPosition)
        {
            var spineZPosition = bodyPosition.Z;

            return spineZPosition > 1.55 && spineZPosition < 4.5;
        }

        private double TransformToAppVolume(CameraSpacePoint bodyPosition)
        {
            var spineZPosition = bodyPosition.Z;

            return (int)spineZPosition;
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
                // y = 1 + (x-A)*(10-1)/(B-A)
                var scaledDist = 1 + (totalDist - 0) * (10 - 1) / (10 - 1);
                return scaledDist;
            }
            else
            {
                this._pastPosition.Add(id, bodyPosition);
                return 5;
            }
        }


        public event EventHandler<IEnumerable<Model.Action>> PlayerActionsChanged;
    }
}
