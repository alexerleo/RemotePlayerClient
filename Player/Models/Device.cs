using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Player.Database;
using System.Runtime.Serialization;

using Player.Database;

namespace Player.Models
{
    /// <summary>
    /// Device info
    /// </summary>
    [DataContract]
    public class Device
    {
        /// <summary>
        /// Device info
        /// </summary>
        [DataMember]
        public DeviceInfo info { get; set; }

        /// <summary>
        /// Descibes local device
        /// </summary>
        [IgnoreDataMember]
        public bool isLocal { get; } = true;

        [IgnoreDataMember]
        public string name { get; set; } = "Local"; 

        /// <summary>
        /// Devices database context
        /// </summary>
        [DataMember]
        public MusicCollection collection { get; set; }
    }
}
