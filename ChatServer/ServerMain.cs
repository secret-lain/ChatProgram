using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ChatProgram
{
    partial class ServerMain
    {
        private static TcpListener serverSocket;
        private static IPAddress ip;
        private static int port;
        private static Queue<byte[]> clientSentMsgQueue = new Queue<byte[]>(5);
        private static Dictionary<String,TcpClient> clientPool = new Dictionary<String,TcpClient>(10);
        private static LinkedList<ClientNode> clientPoolTest = new LinkedList<ClientNode>();//TODO ForTest
        private const int BUFFERSIZE = 512;
        //clientPool은 clientId 없이 모든 클라이언트에게 메세지 브로드캐스팅을 하기위함
        
        static void Main(string[] args)
        {
            Console.WriteLine("Chatting Server start...");
            Console.WriteLine("Argument Check(ip, port)...");

            //Argument가 ip, port 순으로 존재하면 사용하고, 그렇지 않으면 localhost:25252
            if (args.Length == 2)
            {
                if(IPAddress.TryParse(args[0],out ip) && Int32.TryParse(args[1],out port)){
                    Console.WriteLine("Argument is correct, server address is " + ip + ":" + port);
                }
            }
            else
            {
                ip = IPAddress.Parse("127.0.0.1");
                port = 25252;
                Console.WriteLine("Argument check failed, server address is " + ip + ":" + port);
                //Console.WriteLine("but your ip is" + Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]);
            }

            serverSocket = new TcpListener(ip, port);
            serverSocket.Start();

            //서버에 상주하며 메세지큐를 검사하고 메세지를 브로드캐스팅해주는 쓰레드. 람다로 구현.
            Thread MsgQueueThread = new Thread(new ThreadStart(()=>
            {
                //try
                //{
                    while (true)
                    {
                        if (clientSentMsgQueue.Count > 0)
                        {
                            byte[] dequeBytes = clientSentMsgQueue.Dequeue();
                            String dequeEchoString = Encoding.UTF8.GetString(dequeBytes).TrimEnd(new char[] { (char)0 });
                            if(dequeEchoString != "")
                            {
                                Console.WriteLine(dequeEchoString);//Echo

                                foreach (TcpClient node in clientPool.Values.ToArray())
                                {
                                    NetworkStream ns = node.GetStream();
                                    
                                    if (ns == null || !ns.CanWrite) continue;
                                    if (dequeEchoString != null)
                                    {
                                       ns.Write(dequeBytes, 0, BUFFERSIZE);
                                    }
                                }
                            }
                            
                        }
                    }
                //}
                //catch (InvalidOperationException) { }
                //catch (IOException) { }
                //Exception의 경우 Node Pool에서 알아서 삭제되기 때문에 처리하지 않는다.
                //물론 예외가 난 경우 Thread가 멈춰버리는 문제가 있음. 해결필요
            }));
            MsgQueueThread.Start();

            //지속적으로 Tcp Socket을 Accept 하는 부분
            //accepted 시 닉네임 중복 검색 -> 중복없을시 1 -> 접속알림 브로드캐스팅 -> 스레드생성
            while (true)
            {
                //접속시 최초에 클라이언트에서 설정한 닉네임을 받는다
                byte[] nickNameCheckBuffer = new byte[BUFFERSIZE];
                String nickNameCheckBufferString;
                TcpClient acceptedClient = serverSocket.AcceptTcpClient();
                acceptedClient.GetStream().Read(nickNameCheckBuffer, 0, BUFFERSIZE);
                nickNameCheckBufferString = Encoding.UTF8.GetString(nickNameCheckBuffer).TrimEnd(new char[] { (char)0 });
                
                //해당 닉네임과 같은 닉네임이 clientPool 에 존재할 경우 0의 값을 전송, 없을 경우 1의 값을 전송 후 accept
                //TODO 존재할 경우 임의의 숫자를 붙인 닉네임을 전송하고 accept
                if (clientPool.ContainsKey(nickNameCheckBufferString))
                {
                    Console.WriteLine(nickNameCheckBufferString + "'s connection is rejected - exist same nickname on server");
                    acceptedClient.GetStream().Write(new byte[] { 0 }, 0, 1);
                    acceptedClient.Close();
                }
                else
                {   
                    if (acceptedClient.Connected)
                    {
                        Console.WriteLine("Client, " + nickNameCheckBufferString + " is accepted at " + DateTime.Now);
                        acceptedClient.GetStream().Write(new byte[] { 1 }, 0, 1);
                        clientSentMsgQueue.Enqueue(Encoding.UTF8.GetBytes((nickNameCheckBufferString + " is entered.").PadRight(BUFFERSIZE,(char)0)));
                        clientPool.Add(nickNameCheckBufferString, acceptedClient);
                        
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ClientCommFunction), nickNameCheckBufferString);
                    }
                }
            }
        }
    }
    
    //client Methods + other Methods
    partial class ServerMain
    {
        //클라이언트의 Msg Recv Thread. 에러발생시 처리
        static void ClientCommFunction(object _clientId)
        {
            String clientId = (String)_clientId;
            TcpClient socket = (TcpClient)clientPool[clientId];
            NetworkStream socketStream = socket.GetStream();

            try
            {
                //string recv phase
                while (true)
                {
                    if (socket.Client.Poll(1000, SelectMode.SelectRead) && socket.Client.Available == 0)
                    {
                        Console.WriteLine(clientId + "'s Connection close");
                        clientSentMsgQueue.Enqueue(Encoding.UTF8.GetBytes((clientId + " left.").PadRight(BUFFERSIZE, (char)0)));
                        
                        break;
                    }
                    else
                    {
                        byte[] recvMsgByte = new byte[BUFFERSIZE];
                        socket.GetStream().Read(recvMsgByte, 0, BUFFERSIZE);
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
                Console.WriteLine("Socket connection Closed - IOException Error");
            }
            finally
            {
                socket.Close();
                clientPool.Remove(clientId);
            }
        }
    }
}
