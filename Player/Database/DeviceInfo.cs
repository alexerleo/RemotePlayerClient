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
    /// Remote device model
    /// </summary>
    [DataContract]
    public class DeviceInfo
    {
        /// <summary>
        /// Device id
        /// </summary>
        [DataMember]
        public Guid id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string name { get; set; } = "Unnamed";
    }
}
