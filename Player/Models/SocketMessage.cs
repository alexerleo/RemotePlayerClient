using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.Serialization;
using Windows.Storage.Streams;
using System.IO;

namespace Player.Models
{
    /// <summary>
    /// Socket message
    /// </summary>
    [DataContract]
    public class SocketMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        [DataMember]
        public string message { get; set; }

        /// <summary>
        /// Email for auth
        /// </summary>
        [DataMember]
        public string email { get; set; }        

        /// <summary>
        /// Password for auth
        /// </summary>
        [DataMember]
        public string password { get; set; } 

        /// <summary>
        /// Audio file
        /// </summary>
        [DataMember]
        protected Byte[] _stream { get; set; }

        [IgnoreDataMember]
        public IRandomAccessStream stream
        {            
            set
            {
                Stream str = value.AsStreamForRead();
                long len = str.Length;
                int slen = (int)len;
                var bytes = new byte[slen];
                str.Read(bytes, 0, slen);
                _stream = bytes;
            }
        }

        /// <summary>
        /// Query string that contains track alias to be recieved
        /// </summary>
        [DataMember]
        public string alias { get; set; }

        /// <summary>
        /// Device id to recieve track
        /// </summary>
        [DataMember]
        public Guid? deviceId { get; set; }

        [DataMember]
        public string contentType { get; set; }
        
        /// <summary>
        /// Devices list
        /// </summary>
        [DataMember]
        public List<Device> devices { get; set; } 
        
        /// <summary>
        /// Get file streeam
        /// </summary>
        /// <returns>File stream</returns>
        public async Task<IRandomAccessStream> GetStream()
        {
            return await bytesToStream(_stream);
        }

        /// <summary>
        /// Get file stream from bytes array
        /// </summary>
        /// <param name="bytes">Serialized stream</param>
        /// <returns>File random access stream</returns>
        private async Task<IRandomAccessStream> bytesToStream(byte[] bytes)
        {
            StorageFile file;
            IRandomAccessStream filestream = null;
            try
            {
                file = await KnownFolders.MusicLibrary.GetFileAsync("RecievedFile");
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                try
                {
                    file = await KnownFolders.MusicLibrary.CreateFileAsync("RecievedFile");
                    filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    await filestream.AsStreamForWrite().WriteAsync(bytes, 0, bytes.Length); 
                }
                catch(Exception)
                {
                    //something goes wrong
                }
            }
                       
            return filestream;
        }

        /// <summary>
        /// Prepare message for send to server
        /// </summary>
        /// <param name="msg">Socket message</param>
        /// <returns>Serialized message as string</returns>
        public static string Serialize(SocketMessage msg)
        {            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    DataContractSerializer serializer = new DataContractSerializer(msg.GetType());
                    serializer.WriteObject(memoryStream, msg);   
                    
                    memoryStream.Position = 0;
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserealize message from server
        /// </summary>
        /// <param name="xml">Recieved message</param>
        /// <returns>Socket message</returns>
        public static SocketMessage Deserialize(string xml)
        {
            SocketMessage msg = new SocketMessage();
            msg.message = "401";
            string xml2compare = SocketMessage.Serialize(msg);
            using (Stream stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(xml2compare);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(SocketMessage));
                var res = (SocketMessage)deserializer.ReadObject(stream);
            }

            using (Stream stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(SocketMessage));
                return (SocketMessage)deserializer.ReadObject(stream);
            }
        }
    }    
}
