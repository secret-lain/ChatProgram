namespace ChatProgram.Sources
{
    partial class ConnectedClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectedClientListbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ConnectedClientListbox
            // 
            this.ConnectedClientListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectedClientListbox.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ConnectedClientListbox.Location = new System.Drawing.Point(5, 5);
            this.ConnectedClientListbox.Name = "ConnectedClientListbox";
            this.ConnectedClientListbox.Size = new System.Drawing.Size(185, 452);
            this.ConnectedClientListbox.TabIndex = 0;
            this.ConnectedClientListbox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ConnectedClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 462);
            this.ControlBox = false;
            this.Controls.Add(this.ConnectedClientListbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConnectedClientForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ConnectedClientForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ConnectedClientListbox;
    }
}