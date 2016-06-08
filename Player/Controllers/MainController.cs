using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

using Player.Models;
using Player.Database;
using Player.Utils;

namespace Player.Controllers
{
    public class MainController
    {
        public MainController()
        {
            Library.InitCollection();
            devices.AddRange(Library.devices);
            current.device = devices.FirstOrDefault(x => x.isLocal == true);
            tracks.AddRange(current.device.collection.tracks);
            bands.AddRange(current.device.collection.bands);
            albums.AddRange(current.device.collection.albums);
        }

        private Playlist playlist = new Playlist();

        public Library Library { get; private set; } = new Library();

        public ObservableCollectionEx<Device> devices { get; set; } = new ObservableCollectionEx<Device>();
        public ObservableCollectionEx<Track> tracks { get; set; } = new ObservableCollectionEx<Track>();
        public ObservableCollectionEx<Album> albums { get; set; } = new ObservableCollectionEx<Album>();
        public ObservableCollectionEx<Band> bands { get; set; } = new ObservableCollectionEx<Band>();
        public SelectionModel current { get; private set; } = new SelectionModel();

        public void NextTrack()
        {
            current.track = playlist.next();
            playNext();
        }

        public void PrevTrack()
        {
            current.track = playlist.prev();
            playNext();
        }

        private void playNext()
        {
            if (current.track == null)
                return;
            var _next = tracks.FirstOrDefault(x => x.alias == current.track?.alias);
            if (_next == null)
                Library.SetPlayItem(current.track);
            else            
                current.track = _next;
        }

        public async void SelectTrack(Track track)
        {
            playlist.Set(tracks.ToList());
            current.track = track;
            playlist.current = current.track;
            IRandomAccessStream stream;
            if (current.device.isLocal)
            {
                var file = await Library.GetPlayFile(track.alias);
                stream = await file.OpenAsync(FileAccessMode.Read);
                Library.SetPlayItem(stream, file.ContentType);
            }
            else
                SocketController.instance.GetTrack(current.device.info.id, track.alias);            
        }

        public void SelectAlbum(Album album)
        {
            tracks.Clear();
            tracks.AddRange(album.tracks);
            current.album = album;
        }
        
        public void SelectBand(Band band)
        {
            albums.Clear();
            albums.AddRange(band.albums.ToList());
            List<Track> _tracks = new List<Track>();
            foreach (var album in albums.Select(x => x.tracks))
            {
                _tracks = _tracks.Concat(album).ToList();
            }
            tracks.Clear();
            tracks.AddRange(_tracks);
            current.band = band;
        }

        public void SelectDevice(Device device)
        {
            bands.Clear();
            albums.Clear();
            tracks.Clear();
            bands.AddRange(device.collection.bands);
            albums.AddRange(device.collection.albums);
            tracks.AddRange(device.collection.tracks);
            current.device = device;
        }        
    }
}
