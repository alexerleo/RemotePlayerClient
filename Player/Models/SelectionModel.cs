using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Player.Database;

namespace Player.Models
{
    public class SelectionModel : INotifyPropertyChanged
    {
        private Track _track;
        private Album _album;
        private Band _band;
        private Device _device;

        public Track track
        {
            get
            {
                return _track;
            }
            set
            {
                if (value != null)
                {
                    _track = value;
                    NotifyPropertyChanged("track");
                }
            }
        }

        public Album album
        {
            get
            {
                return _album;
            }
            set
            {
                if (value != null)
                {
                    _album = value;
                    NotifyPropertyChanged("album");
                }
            }
        }

        public Band band
        {
            get
            {
                return _band;
            }
            set
            {
                if (value != null)
                {
                    _band = value;
                    NotifyPropertyChanged("band");
                }
            }
        }

        public Device device
        {
            get
            {
                return _device;
            }
            set
            {
                if (value != null)
                {
                    _device = value;
                    NotifyPropertyChanged("device");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
