using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Player.Interfaces
{
    /// <summary>
    /// Music collection contracts
    /// </summary>
    interface IMusicCollection
    {
        /// <summary>
        /// Update database
        /// </summary>        
        Task Update();

        /// <summary>
        /// Fill database
        /// </summary>
        Task Fill();

        /// <summary>
        /// Retrive track file by track alias
        /// </summary>
        /// <param name="alias">Track alias</param>
        /// <returns>Audio file</returns>
        Task<StorageFile> GetTrack(string alias);
    }
}
