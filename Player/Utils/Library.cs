using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Player.Models;
using Player.Database;
using Player.Controllers;

namespace Player.Utils
{
    public class Library
    {
        public Library()
        {
            devices.Add(new Device()
            {
                collection = new MusicCollection(collection.db),
                info = collection.db.devices.FirstOrDefault()
            });
        }

        public delegate void PlayingEvent(IRandomAccessStream stream, string contentType);
        public event PlayingEvent Playing;        

        #region callbacks
        private void OnAuth(string message)
        {
            Device device = devices.First(x => x.isLocal == true);
            device.info = collection.db.devices.First();
            Socket.SendDevice(device);
        }
        
        private async void OnGetTrack(string alias, Guid device_id)
        {
            StorageFile file = await GetPlayFile(alias);
            IRandomAccessStream stream = await file.OpenReadAsync();
            string contentType = file.ContentType;
            Socket.SendTrack(stream, contentType, device_id);
        }

        private void OnTrack(IRandomAccessStream stream, string contentType)
        {
            Playing(stream, contentType);
        }

        private void OnDevices(List<Device> devices)
        {
            foreach (Device device in devices)
            {
                device.name = device.info.name;
            }
            devices.AddRange(devices);
        }
        #endregion

        public static Collection collection = new Collection();

        public SocketController Socket = SocketController.instance;

        public List<Device> devices { get; set; } = new List<Device>();        

        public async Task<StorageFile> GetPlayFile(string alias)
        {
            return await collection.GetTrack(alias);
        }

        public async Task<IRandomAccessStream> GetPlayStream(string alias)
        {
            StorageFile file = await GetPlayFile(alias);
            return await file.OpenAsync(FileAccessMode.Read);
        }

        public void SetPlayItem(IRandomAccessStream stream, string contentType)
        {
            Playing(stream, contentType);
        }

        public async void SetPlayItem(SocketMessage message)
        {
            Playing(null, "");
            SetPlayItem(await message.GetStream(), message.contentType);
        }

        public void SetPlayItem(string message)
        {
            SocketMessage decoded = SocketMessage.Deserialize(message);
            SetPlayItem(decoded);
        }

        public async void SetPlayItem(Track track)
        {
            var file = await GetPlayFile(track.alias);
            SetPlayItem(await file.OpenAsync(FileAccessMode.Read), file.ContentType);
        }

        public async void InitCollection()
        {
            Socket.callbacks.onGetTrack += OnGetTrack;
            Socket.callbacks.onTrack += OnTrack;
            Socket.callbacks.onDevices += OnDevices;
            Socket.callbacks.onAuth += OnAuth;

            await collection.Update();                
        }
    }
}