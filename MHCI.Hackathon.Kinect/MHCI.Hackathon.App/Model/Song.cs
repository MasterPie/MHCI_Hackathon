using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Hackathon.App.Model
{
    class Song
    {
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
