using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Hackathon.App.Model
{
    class Song
    {
        public Song(string name, string fileLocation)
        {
            this._name = name;
            this._fileLocation = fileLocation;
        } 
        private string _fileLocation;
        public string FileLocation
        {
            get
            {
                return _fileLocation;
            }
            set
            {
                _fileLocation = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}
