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
    /// Artist model
    /// </summary>
    [DataContract(IsReference = true)]
    public class Band
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int id { get; set; }

        /// <summary>
        /// Band name from ID3 tags
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// List of own albums
        /// </summary>
        [DataMember]
        public List<Album> albums { get; set; }       
    }
}
