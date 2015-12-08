using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

namespace ChatProgram
{
    class ClientNode
    {
        private TcpClient client;
        private NetworkStream nStream;

        public int clientLevel { get; set; }
        public int BUFFERSIZE { get; set; }
        
        private String clientId;
        public String setClientId { set { clientId = value; } }
        //ClientId는 Server의 Hashtable이 Key로서 관리
        //아직 사용되지 않음
        
        //ClientNode에서 사용되는 client는 기본적으로 Connection이 완료된상태
        public ClientNode(TcpClient client,int clientLevel)
        {
            this.client = client;
            BUFFERSIZE = 512;
        }

        //client의 연결여부를 검증하고, NetworkStream을 얻는다. ReceiveTimeout = 10s
        private void Connect()
        {
            if (client.Connected){
                client.ReceiveTimeout = 5000;
                client.SendTimeout = 5000;
                nStream = client.GetStream();
            }
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Write
        /// </summary>
        /// <param name="message">보낼 문자열</param>
        /// <returns>true = 전송성공, false = 전송실패</returns>
        public Boolean Write(String message, int customBufferSize = -1)
        {
            try
            {
                if (nStream.CanWrite)
                {
                    byte[] msgByte;

                    if(customBufferSize != -1)
                        msgByte = new byte[customBufferSize];
                    else
                        msgByte = new byte[BUFFERSIZE];

                    Encoding.UTF8.GetBytes(message).CopyTo(msgByte, 0);
                    nStream.Write(msgByte, 0, msgByte.Length);
                    return true;
                }
                return false;
            }
            catch { throw; }
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Write
        /// </summary>
        /// <param name="message">보낼 문자열</param>
        /// <returns>true = 전송성공, false = 전송실패</returns>
        public Boolean Write(byte[] message, int customBufferSize = -1)
        {
            try
            {
                if (nStream.CanWrite)
                {
                    if (customBufferSize != -1)
                        nStream.Write(message, 0, customBufferSize);
                    else nStream.Write(message, 0, BUFFERSIZE);

                    return true;
                }
                return false;
            }
            catch { throw; }
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Read
        /// </summary>
        /// <returns>byte[] = 받은메세지, null = 실패(Timeout)</returns>
        public byte[] Read(int customBufferSize = -1)
        {
            try
            {
                byte[] result = new byte[256];
                if (nStream.CanRead)
                {
                    if (customBufferSize != -1)
                        nStream.Read(result, 0, customBufferSize);
                    else nStream.Read(result, 0, BUFFERSIZE);

                    return result;
                }
                else return null;
            }
            catch { throw; }
        }

        /// <summary>
        /// Node에 할당된 Stream을 통해 Read
        /// </summary>
        /// <returns>byte[] = 받은메세지, null = 실패(Timeout)</returns>
        public String ReadString()
        {
            try
            {
                byte[] result = this.Read();
                if (result != null)
                {
                    String resultString = Encoding.UTF8.GetString(result).TrimEnd(new char[] { (char)0 });
                    return resultString;
                }
                else return null;
            }
            catch { throw; }
        }
    }
}
