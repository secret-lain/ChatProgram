using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ChatServer
{
    public partial class ClientForm : Form
    {
        TcpClient clientSocket = null;
        NetworkStream dataStream = null;
        String client_id = null;
        Thread ServerMsgRecvThread = null;

        private Boolean ClientConnect()
        {
            try
            {
                TextWrite(">> Connecting..");
                clientSocket = new TcpClient("127.0.0.1", 25252);
                dataStream = clientSocket.GetStream();
                //dataStream = new StreamWriter(clientSocket.GetStream(), Encoding.GetEncoding("ks_c_5601-1987"));
                //dataStream.AutoFlush = true;

                TextWrite(">> Server Connected!");
                button1.Visible = false;

                if (ServerMsgRecvThread == null)
                {
                    ServerMsgRecvThread = new Thread(new ThreadStart(() =>
                    {
                        byte[] recvMsg = new byte[256];
                        while (true)
                        {
                            dataStream.Read(recvMsg, 0, recvMsg.Length);
                            String parsedRecvMsg = new String(Encoding.UTF8.GetChars(recvMsg)).TrimEnd(new char[] { (char)0 });
                            ContentTextbox.AppendText(parsedRecvMsg + "\n");
                            recvMsg.Initialize();
                        }
                    }));
                    ServerMsgRecvThread.Start();
                }
                return true;
            } catch(SocketException){
                TextWrite(">> Connect Failed!");
                button1.Visible = true;
                if (ServerMsgRecvThread != null && ServerMsgRecvThread.IsAlive)
                    ServerMsgRecvThread.Abort();
                return false;
            }
        }

        //TODO 소켓Write
        //ContentTextbox 에 입력텍스트를 출력하고 소켓을 통해 서버에 같은 메시지 전송
        private void TextWrite(String text, Boolean printId = false, Boolean serverSend = false)
        {
            try
            {
                if (printId)
                    text = "[" + client_id + "] " + text;

                if (dataStream != null && serverSend)
                {
                    byte[] sendMsg = new byte[256];
                    Encoding.UTF8.GetBytes(text).CopyTo(sendMsg, 0);

                    dataStream.Write(sendMsg, 0, sendMsg.Length);
                    dataStream.Flush();
                }
                else if(!serverSend)
                    ContentTextbox.AppendText(text + "\n");

            }
            catch (SocketException)
            {
                ContentTextbox.AppendText("\n>> Socket Connection Failed Unexpectably! Please click Reconnect\n");
                SendMsgTextbox.Clear();
                CloseSocket();
                button1.Visible = true;
            }
            catch (IOException)
            {
                ContentTextbox.AppendText("\n>> Socket Connection Failed Unexpectably! Please click Reconnect\n");
                SendMsgTextbox.Clear();
                CloseSocket();
                button1.Visible = true;
            }
        }
        

        public ClientForm()
        {
            InitializeComponent();
        }

        //Form은 실행시 자동으로 연결을 시도
        private void Form1_Load(object sender, EventArgs e)
        {
            ClientConnect();
        }

        // Send 버튼 클릭시 TextWrite를 통해 서버로 메세지 전송.
        private void SendButton_Click(object sender, EventArgs e)
        {
                //자기닉네임을 결정하지 않았을 경우 MsgBox 출력
                if (client_id == null)
                    MessageBox.Show("You should input your id first", "Error");
                else
                {
                    //내용이 없을 경우 아무 반응 안함, 내용이 있을 경우 출력
                    if (SendMsgTextbox.Text != null && !SendMsgTextbox.Text.Equals(""))
                    {
                        TextWrite(SendMsgTextbox.Text,true,true);
                        SendMsgTextbox.Clear();
                    }
                }
        }

        private void CloseSocket()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            if (dataStream != null)
            {
                dataStream.Close();
                dataStream = null;
            }
            if (ServerMsgRecvThread != null)
            {
                ServerMsgRecvThread.Abort();
                ServerMsgRecvThread = null;
            }
            
        }

        //Form 닫힐 시 연결종료
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseSocket();
        }

        //Reconnect 활성화 후 클릭시 연결시도
        private void button1_Click(object sender, EventArgs e)
        {
            ClientConnect();
        }

        //인장 클릭시 블로그이동
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://blog.naver.com/vvvb78");
        }

        //입력란에 커서 위치시 닉네임 변경여부 확인 및 서버에 변경상태 전달
        private void SendMsgTextbox_Enter(object sender, EventArgs e)
        {
            if ((client_id != idTextbox.Text) && !idTextbox.Text.Equals(""))
            {
                if (client_id == null)
                    TextWrite(">> Your Nickname is " + idTextbox.Text);
                else TextWrite(client_id + "->" + idTextbox.Text,serverSend:true);
                
                client_id = idTextbox.Text;
            }
        }

        private void ContentTextbox_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (mouseEvent.Button == MouseButtons.Left)
            {
                DialogResult result = MessageBox.Show("Clear Log?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (result == DialogResult.Yes)
                {
                    ContentTextbox.Clear();
                }
            }
        }

        private void SendMsgTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendButton_Click(sender, e);
            }
        }
    }
}
