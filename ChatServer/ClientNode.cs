using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

namespace ChatClient
{
    class ClientNode
    {
        private TcpClient client;
        private NetworkStream nStream;

        public int clientLevel { get; set; }
        //ClientId는 Server의 Hashtable이 Key로서 관리
        
        public ClientNode(TcpClient client,int clientLevel)
        {
            this.client = client;
            Connect();
        }

        //client의 연결여부를 검증하고, NetworkStream을 얻는다. ReceiveTimeout = 10s
        private void Connect()
        {
            if (client.Connected){
                client.ReceiveTimeout = 10000;
                client.SendTimeout = 10000;
                nStream = client.GetStream();
            }
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Write
        /// </summary>
        /// <param name="message">보낼 문자열</param>
        /// <returns>true = 전송성공, false = 전송실패</returns>
        public Boolean Write(String message)
        {
            if (nStream.CanWrite)
            {
                byte[] msgByte = new byte[256];
                Encoding.UTF8.GetBytes(message).CopyTo(msgByte, 0);
                nStream.Write(msgByte, 0, msgByte.Length);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Write
        /// </summary>
        /// <param name="message">보낼 문자열</param>
        /// <returns>true = 전송성공, false = 전송실패</returns>
        public Boolean Write(byte[] message)
        {
            if (nStream.CanWrite)
            {
                nStream.Write(message, 0, 256);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Read
        /// </summary>
        /// <returns>byte[] = 받은메세지, null = 실패(Timeout)</returns>
        public byte[] Read()
        {
            byte[] result = new byte[256];
            if (nStream.CanRead)
            {
                nStream.Read(result, 0, 256);
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Read
        /// </summary>
        /// <returns>byte[] = 받은메세지, null = 실패(Timeout)</returns>
        public String ReadString()
        {
            byte[] result = this.Read();
            if (result != null)
            {
                String resultString = Encoding.UTF8.GetString(result).TrimEnd(new char[] { (char)0 });
                return resultString;
            }
            else return null;
        }
    }
}
