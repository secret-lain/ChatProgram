using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ChatServer
{
    //main Methods
    partial class ServerMain
    {
        private static TcpListener serverSocket;
        private static IPAddress ip;
        private static Queue<byte[]> clientSentMsgQueue = new Queue<byte[]>(5);
        private static HashSet<TcpClient> clientPool = new HashSet<TcpClient>();
        //clientPool은 clientId 없이 모든 클라이언트에게 메세지 브로드캐스팅을 하기위함

        static void Main(string[] args)
        {
            ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new TcpListener(ip, 25252);
            serverSocket.Start();

            //서버에 상주하며 메세지큐를 검사하고 메세지를 브로드캐스팅해주는 쓰레드. 람다로 구현.
            Thread MsgQueueThread = new Thread(new ThreadStart(()=>
            {
                try
                {
                    while (true)
                    {
                        if (clientSentMsgQueue.Count > 0)
                        {
                            byte[] dequeString = clientSentMsgQueue.Dequeue();
                            foreach (TcpClient node in clientPool)
                            {
                                if(node.Connected)
                                    node.GetStream().Write(dequeString, 0, dequeString.Length);
                            }
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    //Pool에서의 Remove는 Recv Thread에서 처리를 담당한다.
                }
            }));
            MsgQueueThread.Start();

            //지속적으로 Tcp Socket을 Accept 하는 부분
            while (true)
            {
                TcpClient acceptedClient = serverSocket.AcceptTcpClient();
                Console.WriteLine("Client Accepted at " + DateTime.Now);
                if (acceptedClient.Connected)
                {
                    clientPool.Add(acceptedClient);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientCommFunction), acceptedClient);
                }
            }
        }
    }
    
    //client Methods + other Methods
    partial class ServerMain
    {
        //클라이언트의 Msg Recv Thread. 에러발생시 
        static void ClientCommFunction(object _socket)
        {
            TcpClient socket = (TcpClient)_socket;
            try
            {
                //string recv phase
                while (true)
                {
                    byte[] recvMsgByte = new byte[256];
                    socket.GetStream().Read(recvMsgByte, 0, socket.ReceiveBufferSize);
                    if (recvMsgByte != null)
                    {
                        //String parsedRecvMsg = new String(Encoding.UTF8.GetChars(recvMsgByte)).TrimEnd(new char[] { (char)0 });
                        clientSentMsgQueue.Enqueue(recvMsgByte);
                    }
                    else
                    {
                        Console.WriteLine("ClientComm - read Error\n");
                        break;
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (IOException)
            {
                Console.WriteLine("Socket connection Closed");
            }
            finally
            {
                clientPool.Remove(socket);
                socket.Close();
            }
        }
    }
}
