using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player.Database;

namespace Player.Utils
{
    public class Playlist
    {
        private List<Track> tracks;
        public Track current { get; set; }
        public Track next()
        {
            current = tracks.FirstOrDefault(x => x.alias == current.alias);
            var index = tracks.IndexOf(current);
            if (index >= tracks.Count)
                return null;
            var next = tracks[++index];
            current = next;
            return next;
        }
        public Track prev()
        {
            current = tracks.FirstOrDefault(x => x.alias == current.alias);
            var index = tracks.IndexOf(current);
            if (index == 0)
                return null;
            var prev = tracks[--index];
            current = prev;
            return prev;
        }
        public void Set(List<Track> tracks)
        {
            if (this.tracks?.Equals(tracks) ?? false)
                return;
            this.tracks = tracks;
        }
    }
}
