using System.Net.Sockets;
using System.Net;
using System;

namespace ChatServer
{
    class ClientNode
    {
        private TcpClient client;
        private NetworkStream nStream;

        /// <summary>
        /// status
        /// 0 = initialized, not used
        /// 1 = Connected
        /// -1 = notConnected, error
        /// </summary>
        public int statusFlag{ get{ return _statusFlag; } }
        private int _statusFlag;
        public String clientId { get { return _clientId; } }
        private String _clientId;
        
        public ClientNode(TcpClient client, String clientId)
        {
            this._clientId = clientId;
            this.client = client;
            _statusFlag = 0;
            Connect();
        }

        //client의 연결여부를 검증하고, NetworkStream을 얻는다.
        private void Connect()
        {
            if (client.Connected){
                nStream = client.GetStream();
                _statusFlag = 1;
            }
            else _statusFlag = -1;
        }
        
    }
}
