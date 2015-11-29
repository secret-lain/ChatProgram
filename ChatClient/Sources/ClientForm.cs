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
using System.Net;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        TcpClient clientSocket = null;
        NetworkStream clientStream = null;
        Thread ServerMsgRecvThread = null;

        String clientNickname = null;
        int serverPort;
        IPAddress serverIP;

        delegate void setTextCallback(string text, Boolean printId, Boolean serverSend);

        /// <summary>
        /// </summary>
        /// <returns>
        /// 1 = 정상
        /// -1 = 예외에 의한 에러
        /// -5 = 서버내 동일 닉네임이 존재함에 의한 에러
        /// </returns>
        private int ClientConnect()
        {
            try
            {
                clientSocket.Connect(serverIP, serverPort);
                clientStream = clientSocket.GetStream();

                byte[] nickNameCheck = new byte[1];
                textWrite(clientNickname, serverSend: true);
                clientStream.Read(nickNameCheck, 0, 1);

                //변환이 실패했거나, 성공했음에도 nickNameCheck 실패(동일한 닉네임이 존재)했을 경우
                if(nickNameCheck[0] == 0){
                    return -5;
                }

                //Connect 후 ServerMsgRecvThread의 초기화
                if (ServerMsgRecvThread != null)
                {
                    ServerMsgRecvThread.Abort();
                    ServerMsgRecvThread = null;
                }
                //ServerMsgRecvThread의 동작. 256 BufferSize의 Msg를 지속적으로 수신한다.
                //Read에서 대기하다가 Server에서 Msg를 송신하면 이후코드실행
                ServerMsgRecvThread = new Thread(new ThreadStart(() =>
                {
                    byte[] recvMsg = new byte[256];
                    while (true)
                    {
                        clientStream.Read(recvMsg, 0, recvMsg.Length);
                        String parsedRecvMsg = new String(Encoding.UTF8.GetChars(recvMsg)).TrimEnd(new char[] { (char)0 });
                        if (parsedRecvMsg != null)
                            textWrite(parsedRecvMsg);
                        recvMsg.Initialize();
                    }
                }));
                ServerMsgRecvThread.Start();

                return 1;
            }
            catch (SocketException)
            {
                if (ServerMsgRecvThread != null && ServerMsgRecvThread.IsAlive)
                    ServerMsgRecvThread.Abort();
                return -1;
            }
        }

        private void CloseSocket()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            if (clientStream != null)
            {
                clientStream.Close();
                clientStream = null;
            }
            if (ServerMsgRecvThread != null)
            {
                ServerMsgRecvThread.Abort();
                ServerMsgRecvThread = null;
            }
        }


        //ContentTextbox 에 입력텍스트를 출력하고 소켓을 통해 서버에 같은 메시지 전송
        private void textWrite(String text, Boolean printId = false, Boolean serverSend = false)
        {
            try
            {
                if (ContentTextbox.InvokeRequired)
                {
                    setTextCallback cb = new setTextCallback(textWrite);
                    Invoke(cb,new object[]{text, printId, serverSend});
                }
                else
                {
                    if (printId)
                        text = "[" + clientNickname + "] " + text;

                    if (clientStream != null && clientStream.CanWrite && serverSend)
                    {
                        byte[] sendMsg = new byte[256];
                        Encoding.UTF8.GetBytes(text).CopyTo(sendMsg, 0);

                        clientStream.Write(sendMsg, 0, sendMsg.Length);
                        clientStream.Flush();
                    }
                    else if (!serverSend)
                        //통신불안정으로 서버 브로드캐스팅을 받아오지 못하는 부분이 아닌 명시적으로 내부메세지로 사용하기 위함
                        ContentTextbox.AppendText(text + "\n");
                }  
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

        /// <summary>
        /// 초기화 페이지의 토글. 닫기의 경우 CloseSocket()을 동반한다.
        /// </summary>
        /// <param name="open">true = 열기, false = 닫기</param>
        private void toggleInitializingPage(Boolean open)
        {
            if (open)
            {
                InitializingPanel.Visible = true;
                MessagePanel.Visible = false;
                CloseSocket();
            }
            else
            {
                InitializingPanel.Visible = false;
                MessagePanel.Visible = true;
                idTextbox.Text = clientNickname;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //                         Form Auto Create Event Method                           //
        /////////////////////////////////////////////////////////////////////////////////////
        public ClientForm()
        {
            InitializeComponent();
            clientSocket = new TcpClient();
        }

        // Send 버튼 클릭시 TextWrite를 통해 서버로 메세지 전송.
        private void SendButton_Click(object sender, EventArgs e)
        {
                //자기닉네임을 결정하지 않았을 경우 MsgBox 출력
                if (clientNickname == null)
                    MessageBox.Show("You should input your id first", "Error");
                else
                {
                    //내용이 없을 경우 아무 반응 안함, 내용이 있을 경우 출력
                    if (SendMsgTextbox.Text != null && !SendMsgTextbox.Text.Equals(""))
                    {
                        textWrite(SendMsgTextbox.Text,true,true);
                        SendMsgTextbox.Clear();
                    }
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
        //Notice::현재 미사용
        /*private void SendMsgTextbox_Enter(object sender, EventArgs e)
        {
            if ((clientNickname != idTextbox.Text) && !idTextbox.Text.Equals(""))
            {
                if (clientNickname == null)
                    textWrite(">> Your Nickname is " + idTextbox.Text);
                else textWrite(clientNickname + "->" + idTextbox.Text,serverSend:true);
                
                clientNickname = idTextbox.Text;
            }
        }*/

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

        private void idTextbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            String buf = InputTextDialog.ShowDialog("Nickname", "Input Your Nickname");
            if (buf != null)
            {
                clientNickname = buf;
                idTextbox.Text = clientNickname;
                //TODO 변경한 점은 서버에 보내야한다.
            }
        }

        //최초 패널내의 connect 버튼을 클릭할때 내용체크 및 연결
        private void InitializingButton_Click(object sender, EventArgs e)
        {
            //Nickname은 바로 입력하지만 나머지는 TryParse로 정확한 값인지 검사
            Boolean isCorrectIP = IPAddress.TryParse(InitializingServerIPText.Text,out serverIP);
            Boolean isCorrectPort = Int32.TryParse(InitializingServerPortText.Text,out serverPort);
            clientNickname = InitializingNicknameText.Text;
            
            if (clientNickname.Equals(""))
            {
                MessageBox.Show("Please input Nickname", "Error");
            }
            else if (!isCorrectIP || !isCorrectPort)
            {
                MessageBox.Show("Please input Server Information", "Error");
            }
            else
            {
                int connectionResult = ClientConnect();
                if (connectionResult == 1)
                {
                    toggleInitializingPage(false);
                }
                else if(connectionResult == -1)
                {
                    MessageBox.Show("Server connection Failed!");
                }
                else if (connectionResult == -5)
                {
                    MessageBox.Show("Same Nickname exist on server!");
                }
            }
        }

        //ContentTextBox 에 아무것도 입력하지 못하게 만들기 위함
        private void ContentTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
