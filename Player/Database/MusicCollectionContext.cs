using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using System.Runtime.Serialization;

namespace Player.Database
{
    /// <summary>
    /// Database context
    /// </summary>
    [DataContract]
    public class MusicCollectionContext: DbContext
    {
        public MusicCollectionContext(): base()
        {
            try
            {
                Database.Migrate();
            }
            catch(Exception ex)
            {

            }
        }


        /// <summary>
        /// Bands list
        /// </summary>
        [DataMember]
        public DbSet<Band> bands { get; set; }

        /// <summary>
        /// Albums list
        /// </summary>
        [DataMember]
        public DbSet<Album> albums { get; set; }

        /// <summary>
        /// Tracks list
        /// </summary>
        [DataMember]
        public DbSet<Track> tracks { get; set; }

        /// <summary>
        /// User devices 
        /// </summary>
        [IgnoreDataMember]
        public DbSet<DeviceInfo> devices { get; set; }

        /// <summary>
        /// User info
        /// </summary>
        [IgnoreDataMember]
        public DbSet<User> users { get; set; }

        /// <summary>
        /// Configure database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dirPath = "";
            try
            {
                dirPath += Windows.Storage.ApplicationData.Current.LocalFolder.Path + "/";
            }
            catch(Exception ex)
            {

            }
            string connectionString = "Filename=" + dirPath + "MusicCollection_beta.db";
            optionsBuilder.UseSqlite(connectionString); 
        }
    }
}
