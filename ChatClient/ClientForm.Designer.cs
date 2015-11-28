namespace ChatServer
{
    partial class ClientForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.SendButton = new System.Windows.Forms.Button();
            this.idTextbox = new System.Windows.Forms.TextBox();
            this.SendMsgTextbox = new System.Windows.Forms.TextBox();
            this.ContentTextbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.InitializingPanel = new System.Windows.Forms.Panel();
            this.InitializingConnectionButton = new System.Windows.Forms.Button();
            this.InitializingServerPortText = new System.Windows.Forms.TextBox();
            this.InitializingServerIPText = new System.Windows.Forms.TextBox();
            this.InitializingNicknameText = new System.Windows.Forms.TextBox();
            this.InitializingDestLabel = new System.Windows.Forms.Label();
            this.InitializingIDLabel = new System.Windows.Forms.Label();
            this.MessagePanel = new System.Windows.Forms.Panel();
            this.InitializingPanel.SuspendLayout();
            this.MessagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(325, 4);
            this.SendButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(86, 29);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // idTextbox
            // 
            this.idTextbox.Location = new System.Drawing.Point(0, 4);
            this.idTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.idTextbox.Name = "idTextbox";
            this.idTextbox.ReadOnly = true;
            this.idTextbox.Size = new System.Drawing.Size(114, 25);
            this.idTextbox.TabIndex = 1;
            this.idTextbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.idTextbox_MouseDoubleClick);
            // 
            // SendMsgTextbox
            // 
            this.SendMsgTextbox.Location = new System.Drawing.Point(121, 4);
            this.SendMsgTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SendMsgTextbox.Name = "SendMsgTextbox";
            this.SendMsgTextbox.Size = new System.Drawing.Size(195, 25);
            this.SendMsgTextbox.TabIndex = 2;
            this.SendMsgTextbox.Enter += new System.EventHandler(this.SendMsgTextbox_Enter);
            this.SendMsgTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendMsgTextbox_KeyPress);
            // 
            // ContentTextbox
            // 
            this.ContentTextbox.Location = new System.Drawing.Point(14, 15);
            this.ContentTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ContentTextbox.Multiline = true;
            this.ContentTextbox.Name = "ContentTextbox";
            this.ContentTextbox.ReadOnly = true;
            this.ContentTextbox.ShortcutsEnabled = false;
            this.ContentTextbox.Size = new System.Drawing.Size(411, 476);
            this.ContentTextbox.TabIndex = 3;
            this.ContentTextbox.DoubleClick += new System.EventHandler(this.ContentTextbox_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 40);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 29);
            this.button1.TabIndex = 4;
            this.button1.Text = "Reconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 47);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(193, 15);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Created By 4hyun, for study";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // InitializingPanel
            // 
            this.InitializingPanel.Controls.Add(this.InitializingConnectionButton);
            this.InitializingPanel.Controls.Add(this.InitializingServerPortText);
            this.InitializingPanel.Controls.Add(this.InitializingServerIPText);
            this.InitializingPanel.Controls.Add(this.InitializingNicknameText);
            this.InitializingPanel.Controls.Add(this.InitializingDestLabel);
            this.InitializingPanel.Controls.Add(this.InitializingIDLabel);
            this.InitializingPanel.Location = new System.Drawing.Point(14, 15);
            this.InitializingPanel.Name = "InitializingPanel";
            this.InitializingPanel.Size = new System.Drawing.Size(411, 476);
            this.InitializingPanel.TabIndex = 6;
            // 
            // InitializingConnectionButton
            // 
            this.InitializingConnectionButton.Location = new System.Drawing.Point(162, 395);
            this.InitializingConnectionButton.Name = "InitializingConnectionButton";
            this.InitializingConnectionButton.Size = new System.Drawing.Size(100, 40);
            this.InitializingConnectionButton.TabIndex = 5;
            this.InitializingConnectionButton.Text = "Connect";
            this.InitializingConnectionButton.UseVisualStyleBackColor = true;
            this.InitializingConnectionButton.Click += new System.EventHandler(this.InitializingButton_Click);
            // 
            // InitializingServerPortText
            // 
            this.InitializingServerPortText.Location = new System.Drawing.Point(219, 295);
            this.InitializingServerPortText.Name = "InitializingServerPortText";
            this.InitializingServerPortText.Size = new System.Drawing.Size(97, 25);
            this.InitializingServerPortText.TabIndex = 4;
            this.InitializingServerPortText.Text = "25252";
            // 
            // InitializingServerIPText
            // 
            this.InitializingServerIPText.Location = new System.Drawing.Point(75, 295);
            this.InitializingServerIPText.Name = "InitializingServerIPText";
            this.InitializingServerIPText.Size = new System.Drawing.Size(138, 25);
            this.InitializingServerIPText.TabIndex = 3;
            this.InitializingServerIPText.Text = "127.0.0.1";
            // 
            // InitializingNicknameText
            // 
            this.InitializingNicknameText.Location = new System.Drawing.Point(75, 151);
            this.InitializingNicknameText.Name = "InitializingNicknameText";
            this.InitializingNicknameText.Size = new System.Drawing.Size(174, 25);
            this.InitializingNicknameText.TabIndex = 2;
            // 
            // InitializingDestLabel
            // 
            this.InitializingDestLabel.AutoSize = true;
            this.InitializingDestLabel.Location = new System.Drawing.Point(72, 267);
            this.InitializingDestLabel.Name = "InitializingDestLabel";
            this.InitializingDestLabel.Size = new System.Drawing.Size(177, 15);
            this.InitializingDestLabel.TabIndex = 1;
            this.InitializingDestLabel.Text = "Server IP,Port to Connect";
            // 
            // InitializingIDLabel
            // 
            this.InitializingIDLabel.AutoSize = true;
            this.InitializingIDLabel.Location = new System.Drawing.Point(72, 118);
            this.InitializingIDLabel.Name = "InitializingIDLabel";
            this.InitializingIDLabel.Size = new System.Drawing.Size(141, 15);
            this.InitializingIDLabel.TabIndex = 0;
            this.InitializingIDLabel.Text = "Enter Your Nickname";
            // 
            // MessagePanel
            // 
            this.MessagePanel.AutoSize = true;
            this.MessagePanel.Controls.Add(this.idTextbox);
            this.MessagePanel.Controls.Add(this.linkLabel1);
            this.MessagePanel.Controls.Add(this.SendButton);
            this.MessagePanel.Controls.Add(this.button1);
            this.MessagePanel.Controls.Add(this.SendMsgTextbox);
            this.MessagePanel.Location = new System.Drawing.Point(14, 497);
            this.MessagePanel.Name = "MessagePanel";
            this.MessagePanel.Size = new System.Drawing.Size(414, 73);
            this.MessagePanel.TabIndex = 7;
            this.MessagePanel.Visible = false;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 578);
            this.Controls.Add(this.MessagePanel);
            this.Controls.Add(this.InitializingPanel);
            this.Controls.Add(this.ContentTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "Basic Chatting Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.InitializingPanel.ResumeLayout(false);
            this.InitializingPanel.PerformLayout();
            this.MessagePanel.ResumeLayout(false);
            this.MessagePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox idTextbox;
        private System.Windows.Forms.TextBox SendMsgTextbox;
        private System.Windows.Forms.TextBox ContentTextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel InitializingPanel;
        private System.Windows.Forms.Button InitializingConnectionButton;
        private System.Windows.Forms.TextBox InitializingServerPortText;
        private System.Windows.Forms.TextBox InitializingServerIPText;
        private System.Windows.Forms.TextBox InitializingNicknameText;
        private System.Windows.Forms.Label InitializingDestLabel;
        private System.Windows.Forms.Label InitializingIDLabel;
        private System.Windows.Forms.Panel MessagePanel;
    }
}

