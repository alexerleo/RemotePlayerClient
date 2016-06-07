using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using Windows.Storage.FileProperties;
using System.Text.RegularExpressions;
using Player.Database;
using Player.Models;
using Player.Interfaces;

namespace Player.Utils
{
    public class Collection: IMusicCollection
    {        
    #region Fields
        /// <summary>
        /// Database context
        /// </summary>
        public MusicCollectionContext db { get; set; }

    #endregion

    #region Private data
        /// <summary>
        /// Scanned files
        /// </summary>
        private List<FileModel> files = new List<FileModel>();
    #endregion

    #region Methods
        /// <summary>
        /// Update database
        /// </summary>        
        public async Task Update()
        {
            await Fill();
            await DeleteRecords();
        }

        /// <summary>
        /// Fill database
        /// </summary>
        public async Task Fill()
        {
            await ScanDrives(KnownFolders.MusicLibrary);
            await AddRecords();
            try
            {
                DeviceInfo device = db.devices.First();
            }
            catch(Exception ex)
            {
                db.devices.Add(new DeviceInfo());
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Retrive track file by track alias
        /// </summary>
        /// <param name="alias">Track alias</param>
        /// <returns>Audio file</returns>
        public async Task<StorageFile> GetTrack(string alias)
        {
            Track track = db.tracks.FirstOrDefault(x => x.alias.Equals(alias));
            if (track == null)
                return null;
            StorageFolder folder = await FindTrackAsync(track.path.Split('/').ToList(), KnownFolders.MusicLibrary);
            return await folder.GetFileAsync(track.fileName);
        }
    #endregion

    #region Private methods
        /// <summary>
        /// Retrieve folder by path
        /// </summary>
        /// <param name="path">Path to file from current folder</param>
        /// <param name="folder">Current scanned folder</param>
        /// <returns>Folder by path</returns>
        private async Task<StorageFolder> FindTrackAsync(List<string> path, StorageFolder folder)
        {
            if (path.Count == 0)
                return folder;
            var nextFolderName = path.FirstOrDefault();
            StorageFolder _folder = await folder.GetFolderAsync(nextFolderName);
            path.RemoveAt(0);
            return await FindTrackAsync(path, _folder);
        }

        /// <summary>
        /// Find all audio files in local music collection
        /// </summary>
        /// <param name="folder">Current scanned folder</param>
        /// <param name="path">Path from root folder</param>
        private async Task ScanDrives(StorageFolder folder, string path = "")
        {
            var files = await folder.GetFilesAsync();
            var tmp = files[0];
            files = files.Where(x => (new List<string>() { ".mp3", ".wave" }).Contains(x.FileType)).ToList();
            var folders = await folder.GetFoldersAsync();
            this.files.AddRange(files.Select(x => new FileModel()
            {
                file = x,
                path = path
            }).ToList());
            foreach (var _folder in folders)
                await ScanDrives(_folder, path + (!String.IsNullOrEmpty(path) ? "/" : "") + _folder.Name);
        }

        /// <summary>
        /// Delete fields which binds to unexisted files
        /// </summary>
        private async Task DeleteRecords()
        {
            try {
                List<Track> tracks = db.tracks.ToList();
                for (int i = tracks.Count - 1; i >= 0; i--)
                {
                    Track track = tracks[i];
                    try
                    {
                        StorageFile file = await GetTrack(track.alias);
                    }
                    catch (Exception ex)
                    {
                        Album album = db.albums.FirstOrDefault(x => x == track.album);
                        Band band = db.bands.FirstOrDefault(x => x == album.band);
                        album.tracks.Remove(track);
                        db.tracks.Remove(track);
                        if (album.tracks.Count <= 1 && album.tracks.Contains(track))
                        {
                            db.albums.Remove(album);
                            band.albums.Remove(album);
                        }
                        if (band.albums.Count <= 1 && band.albums.Contains(album))
                        {
                            db.bands.Remove(band);
                        }
                        db.SaveChanges();
                    }//FileNotFoundException
                }//while
            }
            catch(Exception ex)
            {
                
            }
        }

        /// <summary>
        /// Add records to database
        /// </summary>
        private async Task AddRecords()
        {
            try {
                List<string> tracks = db.tracks.Select(x => x.alias).ToList();

                foreach (var item in files)
                {
                    MusicProperties tags = await item.file.Properties.GetMusicPropertiesAsync();
                    String alias = GenerateAlias(item.file, tags);
                    Track track = db.tracks.FirstOrDefault(x => x.alias == alias);
                    if (track != null)
                    {
                        if (!(item.path.Equals(track.path) && item.file.Name.Equals(track.fileName)))
                        {
                            track.fileName = item.file.Name;
                            track.path = item.path;
                        }
                    }
                    else
                    {
                        string artist = String.IsNullOrEmpty(tags.AlbumArtist) ? "Unknown" : tags.AlbumArtist;
                        string artistAlbum = String.IsNullOrEmpty(tags.Album) ? "Unknown" : tags.Album;
                        string title = String.IsNullOrEmpty(tags.Title) ? item.file.Name : tags.Title;

                        track = new Track();
                        track.alias = alias;
                        track.fileName = item.file.Name;
                        track.path = item.path;
                        track.name = title;

                        Album album = db.albums.FirstOrDefault(x => x.name == artistAlbum && x.band.name == artist);
                        if (album == null)
                        {
                            album = new Album();
                            album.name = artistAlbum;
                            album.year = tags.Year;
                            db.albums.Add(album);
                            Band band = db.bands.FirstOrDefault(x => x.name == artist);
                            if (band == null)
                            {
                                band = new Band();
                                band.name = artist;
                                band.albums = new List<Album>();
                                db.bands.Add(band);
                            }//if
                            band.albums.Add(album);
                            album.band = band;
                        }//if
                        if(album.tracks == null)
                            album.tracks = new List<Track>();
                        album.tracks.Add(track);
                        track.album = album;
                        db.tracks.Add(track);
                        db.SaveChanges();
                    }//else 
                }//foreach 
            }
            catch(Exception ex)
            {

            }           
        }

        /// <summary>
        /// Generate track alias
        /// </summary>
        /// <param name="file">Audio file</param>
        /// <param name="tags">Audio file ID3 tags</param>
        /// <returns>Track alias</returns>
        private string GenerateAlias(StorageFile file, MusicProperties tags)
        {
            string band = String.IsNullOrEmpty(tags.AlbumArtist) ? "Unknown" : tags.AlbumArtist;
            string album = String.IsNullOrEmpty(tags.Album) ? "Unknown" : tags.Album;
            string title = String.IsNullOrEmpty(tags.Title) ? file.Name : tags.Title;
            return String.Format("{0}_{1}_{2}", band, album, title);
        }
    #endregion

    #region Constructions
        /// <summary>
        /// Create database entity
        /// </summary>
        public Collection()
        {
            db = new MusicCollectionContext();
            db.Database.EnsureCreated();       
        }
    #endregion
    }
}
