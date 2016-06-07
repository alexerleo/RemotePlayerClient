using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage.Streams;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace Player
{
    public class Client
    {
        private HostName hostName = new HostName("localhost");
        StreamSocketListener listener = new StreamSocketListener();
        private string clientPort = "27075";
        public Client()
        {
            Init();
        }
        public async Task ConnectToAddress(string ip, string port)
        {
            await listener.BindEndpointAsync(new HostName(ip), port);
        }
        public async void Init()
        {
            
            listener.ConnectionReceived += OnConnection;
            
            listener.Control.KeepAlive = false;

            try
            {
                // Connect to the server (by default, the listener we created in the previous step).
                await socket.BindServiceNameAsync(hostName, clientPort);                              
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
        }
        public async void Emit()
        {
            DataWriter writer;
            writer = new DataWriter(socket.OutputStream);
            
            // Write first the length of the string as UINT32 value followed up by the string. 
            // Writing data to the writer will just store data in memory.
            string stringToSend = "Hello";
            writer.WriteUInt32(writer.MeasureString(stringToSend));
            writer.WriteString(stringToSend);

            // Write the locally buffered data to the network.
            try
            {
                await writer.StoreAsync();
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error if fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
                
            }
        }
    }
}
