using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Player.Database
{
    /// <summary>
    /// Audio track info
    /// </summary>
    [DataContract(IsReference = true)]
    public class Track
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int id { get; set; }
        
        /// <summary>
        /// Filename with extension
        /// </summary>
        [IgnoreDataMember]
        public string fileName { get; set; }
        
        /// <summary>
        /// Relative path to file. KnownFolders.MusicLibrary is root folder
        /// </summary>
        [IgnoreDataMember]
        public string path { get; set; }
        
        /// <summary>
        /// Track name from ID3 tags
        /// </summary>
        [IgnoreDataMember]
        public string name { get; set; }

        /// <summary>
        /// Alias binding to track. Unique location independent file name
        /// </summary>
        [DataMember]
        public string alias { get; set; }
        
        /// <summary>
        /// Album track is from
        /// </summary>
        [DataMember]
        public Album album { get; set; }        
    }
}
