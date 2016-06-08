using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using Quobject.SocketIoClientDotNet.Client;
using Player.Models;
using Player.Utils;

namespace Player.Controllers
{
    public class SocketController
    {
        static SocketController()
        {
            instance = new SocketController();
        }

        private SocketController()
        {
            socket = IO.Socket(serverUri);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                callbacks.onConnected += () =>
                {                
                    if (!String.IsNullOrWhiteSpace(authMessage) && (isDisconnected ?? false))
                    {
                        socket.Emit("Auth", authMessage);
                    }
                    isDisconnected = false;
                };
                callbacks.Connected();
            });

            socket.On(Socket.EVENT_DISCONNECT, () =>
            {
                isDisconnected = true;
            });

            socket.On("GetTrack", (data) =>
            {
                callbacks.GetTrack((String)data);
            });

            socket.On("Track", (data) =>
            {
                callbacks.Track((String)data);
            });

            socket.On("Devices", (data) =>
            {
                callbacks.Devices((String)data);
            });

            socket.On("Auth", (data) =>
            {               
                callbacks.Auth((String)data);
            });
        }

        public static SocketController instance;

        private Socket socket;
        private string serverUri = "https://remoteplayerhost.herokuapp.com";
        private string authMessage = "";
        private bool? isDisconnected = null;

        public SocketCallbacks callbacks { get; private set; } = new SocketCallbacks();      
        
        public void SendDevice(Device device)
        {
            SocketMessage msg = new SocketMessage();
            List<Device> devices = new List<Device>();
            devices.Add(device);
            msg.devices = devices;
            string message = SocketMessage.Serialize(msg);
            socket.Emit("Device", message);
        }

        public void GetDevices(Guid deviceId)
        {
            SocketMessage msg = new SocketMessage();
            msg.deviceId = deviceId;
            string message = SocketMessage.Serialize(msg);
            socket.Emit("GetDevices", message);
        }

        public void SendTrack(IRandomAccessStream stream, string contentType, Guid deviceId)
        {
            SocketMessage msg = new SocketMessage();
            msg.stream = stream;
            msg.contentType = contentType;
            msg.deviceId = deviceId;
            string message = SocketMessage.Serialize(msg);
            socket.Emit("Track", message);
        }
        public void GetTrack(Guid deviceId, string alias)
        {
            SocketMessage msg = new SocketMessage();
            msg.alias = alias;
            msg.deviceId = deviceId;
            string message = SocketMessage.Serialize(msg);
            socket.Emit("GetTrack", message);
        }
        public void Auth(string email, string password)
        {
            SocketMessage msg = new SocketMessage();
            msg.email = email;
            msg.password = password;
            authMessage = SocketMessage.Serialize(msg);
            socket.Emit("Auth", authMessage);
        }

        public void ClearCallbacks()
        {
            callbacks = new SocketCallbacks();
        }
    }
}
