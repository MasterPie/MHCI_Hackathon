using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IrrKlang;

namespace MHCI.Hackathon.App.Model
{
    class MusicPlayer
    { 
        private ISoundEngine _engine;
        private ISound _currentlyPlayingSound;

        public MusicPlayer()
        {
            this._engine = new ISoundEngine();
        }

        public void Play(Song song)
        {
            try
            {
                this.Stop();
                this._currentlyPlayingSound = this._engine.Play2D(song.FileLocation, true);
                
                if (this._currentlyPlayingSound == null)
                {
                    throw new ArgumentException("Unable to play song");
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                this._currentlyPlayingSound.Paused = true;
            }
        }
        public void Stop()
        {
            if(HasSoundFile)
            {
                this._currentlyPlayingSound.Stop();
            }
        }

        public bool HasSoundFile
        {
            get
            {
                return this._currentlyPlayingSound != null && !this._currentlyPlayingSound.Finished;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return HasSoundFile && !this._currentlyPlayingSound.Paused;
            }
        }

    }
}
