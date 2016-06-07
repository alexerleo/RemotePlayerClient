using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Player.Models
{
    /// <summary>
    /// Device scan model
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// file instance
        /// </summary>
        public StorageFile file { get; set; }
        /// <summary>
        /// file path from root folder
        /// </summary>
        public string path { get; set; }
    }
}
