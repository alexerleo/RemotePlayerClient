using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using Player.Models;

namespace Player.Utils
{
    public class SocketCallbacks
    {
        #region events
        public delegate void AuthEvent(string message);
        public event AuthEvent onAuth;

        public delegate void TrackEvent(IRandomAccessStream stream, string contentType);
        public event TrackEvent onTrack;

        public delegate void DevicesEvent(List<Device> devices);
        public event DevicesEvent onDevices;

        public delegate void ConnectedEvent();
        public event ConnectedEvent onConnected;

        public delegate void GetTrackEvent(string alias, Guid deviceId);
        public event GetTrackEvent onGetTrack;
        #endregion

        /// <summary>
        /// Socket connected event
        /// </summary>
        public void Connected()
        {
            try
            {
                onConnected();
            }
            catch (Exception ex)
            {
                //no binding listerners
            }            
        }

        /// <summary>
        /// Recieved devices list event
        /// </summary>
        /// <param name="msg">Socket message</param>
        public void Devices(string msg)
        {
            SocketMessage message = SocketMessage.Deserialize(msg);
            try
            {
                onDevices(message.devices);               
            }
            catch (Exception ex)
            {
                //no binding listerners
            }            
        }

        /// <summary>
        /// Recieve track response
        /// </summary>
        /// <param name="msg">Socket message</param>
        public async void Track(string msg)
        {
            SocketMessage message = SocketMessage.Deserialize(msg);            
            try
            {
                onTrack(await message.GetStream(), message.contentType);
            }
            catch (Exception ex)
            {
                //no binding listerners
            }
        }

        /// <summary>
        /// Auth response
        /// </summary>
        /// <param name="msg">Socket message</param>
        public void Auth(string msg)
        {
            SocketMessage message = SocketMessage.Deserialize(msg);
            try
            {
                onAuth(message.message);
            }
            catch(Exception ex)
            {
                //no binding listerners
            }
        }

        /// <summary>
        /// Get track event
        /// </summary>
        /// <param name="msg">Socket message</param>
        public void GetTrack(string msg)
        {
            SocketMessage message = SocketMessage.Deserialize(msg);            
            try
            {
                onGetTrack(message.alias, message.deviceId.Value);
            }
            catch (Exception ex)
            {
                //no binding listerners
            }
        }
    }
}
