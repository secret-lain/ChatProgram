using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    class InputTextDialog
    {
            public static string ShowDialog(string caption, string text)
            {
                Form prompt = new Form();
                prompt.Width = 240;
                prompt.Height = 80;
                prompt.Text = caption;
                TextBox textBox = new TextBox() { Left = 10, Top = 10, Width = 120, TabIndex = 0, TabStop = true };
                Button confirmation = new Button() { Text = "Ok", Left = 135, Width = 80, Top = 10, TabIndex = 0, TabStop = true };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;
                prompt.StartPosition = FormStartPosition.CenterScreen;
                prompt.ShowDialog();
                return textBox.Text;
            }
    }
}
