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
    /// Album model
    /// </summary>
    [DataContract(IsReference = true)]
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int id { get; set; }

        /// <summary>
        /// Album release year
        /// </summary>
        [DataMember]
        public uint year { get; set; }

        /// <summary>
        /// Album name from ID3 tags
        /// </summary>
        [DataMember]
        public string name { get; set; }
        
        /// <summary>
        /// Band is album wich
        /// </summary>
        [DataMember]
        public Band band { get; set; }

        /// <summary>
        /// Album tracks list
        /// </summary>
        [DataMember]
        public List<Track> tracks { get; set; }        
    }
}
