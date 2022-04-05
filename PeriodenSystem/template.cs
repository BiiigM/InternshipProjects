using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PeridoenSystem
{
    public partial class template : UserControl
    {
        private string idx { get; set; }
        private string name { get; set; }
        private string prefix { get; set; }
        private string masse { get; set; }
        private Color color { get; set; }
        public template(string idx, string name, string prefix, string masse, Color color)
        {
            InitializeComponent();
            this.idx = idx;
            this.name = name;
            this.prefix = prefix;
            this.masse = masse;
            this.color = color;
        }
        
        public int Counter { get; set; }

        private void template_Load(object sender, EventArgs e)
        {
            lCounter.Text = idx;
            lName.Text = name;
            lPrefix.Text = prefix;
            lMasse.Text = masse;
            BackColor = color;

            if (lCounter.Text.Equals("*") || lCounter.Text.Equals("**"))
            {
                lCounter.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            }
        }
    }
}