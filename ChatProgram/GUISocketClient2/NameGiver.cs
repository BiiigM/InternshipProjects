using System;
using System.Windows.Forms;

namespace GUISocketClient2
{
    public partial class NameGiver : Form
    {
        private string _name;
        public NameGiver()
        {
            InitializeComponent();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text.Contains("_"))
            {
                MessageBox.Show(@"Gib mir einen Namen JETZT!!", @"Info");
                return;
            }

            _name = textBox1.Text;

            Form chat = new Form1(_name, this);
            chat.Show();
            this.Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show(@"Gib mir einen Namen JETZT!!", @"Info");
                    return;
                }

                _name = textBox1.Text;

                Form chat = new Form1(_name, this);
                chat.Show();
                this.Hide();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '_')
            {
                MessageBox.Show("Unterstriche sind nicht gestattet du Penner.");
                textBox1.Text = "";
                return;
            }
        }
    }
}