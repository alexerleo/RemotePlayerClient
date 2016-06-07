using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using Player.Database;

namespace Player.Models
{
    [DataContract]
    public class MusicCollection
    {
        [DataMember]
        public List<Band> bands { get; set; }

        /// <summary>
        /// Albums list
        /// </summary>
        [DataMember]
        public List<Album> albums { get; set; }

        /// <summary>
        /// Tracks list
        /// </summary>
        [DataMember]
        public List<Track> tracks { get; set; }

        public MusicCollection(MusicCollectionContext context)
        {
            bands = context.bands.ToList();
            albums = context.albums.ToList();
            tracks = context.tracks.ToList();
        }
    }
}
